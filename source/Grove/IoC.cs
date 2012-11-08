namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using Castle.Core;
  using Castle.Facilities.TypedFactory;
  using Castle.MicroKernel.Lifestyle;
  using Castle.MicroKernel.Registration;
  using Castle.MicroKernel.Resolvers;
  using Castle.MicroKernel.Resolvers.SpecializedResolvers;
  using Castle.MicroKernel.SubSystems.Configuration;
  using Castle.Windsor;
  using Core;
  using Core.Dsl;
  using Infrastructure;
  using Ui.DistributeDamage;
  using Ui.Shell;

  public class IoC
  {
    private readonly Configuration _configuration;

    private IWindsorContainer _container;

    private IoC(Configuration configuration)
    {
      _configuration = configuration;
    }

    private IWindsorContainer Container
    {
      get
      {
        return _container
          ?? (_container = CreateContainer());
      }
    }

    public static IoC Test()
    {
      return new IoC(Configuration.Test);
    }

    public static IoC Ui()
    {
      return new IoC(Configuration.Ui);
    }

    public IDisposable BeginScope()
    {
      return Container.BeginScope();
    }

    public T Resolve<T>()
    {
      return Container.Resolve<T>();
    }

    public object Resolve(Type type)
    {
      return Container.Resolve(type);
    }

    public object Resolve(string key, Type type)
    {
      return Container.Resolve(key, type);
    }

    public Array ResolveAll(Type service)
    {
      return Container.ResolveAll(service);
    }

    private IWindsorContainer CreateContainer()
    {
      return new WindsorContainer()
        .Install(new Registration(_configuration));
    }

    private enum Configuration
    {
      Ui,
      Test
    }

    private class Registration : IWindsorInstaller
    {
      private readonly Configuration _configuration;

      public Registration(Configuration configuration)
      {
        _configuration = configuration;
      }

      public void Install(IWindsorContainer container, IConfigurationStore store)
      {
        container.Kernel.Resolver.AddSubResolver(
          new CollectionResolver(container.Kernel, allowEmptyCollections: true));

        container.AddFacility<TypedFactoryFacility>();
        container.Register(Component(typeof (ILazyComponentLoader), typeof (LazyOfTComponentLoader),
          LifestyleType.Singleton));

        if (_configuration == Configuration.Ui)
        {
          RegisterViewModels(container);
          RegisterShell(container);
          RegisterConfiguration(container);

          container.Register(Component(typeof (Match), lifestyle: LifestyleType.Singleton));
          container.Register(Component(typeof (CardPreviews), lifestyle: LifestyleType.Singleton));
          container.Register(Component(typeof (UiDamageDistribution)));
        }

        RegisterCardsSources(container);
        container.Register(Component(typeof (CardDatabase), lifestyle: LifestyleType.Singleton));
      }

      private ComponentRegistration<object> Component(Type service, Type implementation = null,
        LifestyleType lifestyle = LifestyleType.Transient)
      {
        var services = new List<Type> {service};

        if (implementation != null)
          services.Add(implementation);

        implementation = implementation ?? service;

        var registration = Castle.MicroKernel.Registration.Component
          .For(services)
          .ImplementedBy(implementation);

        return registration.LifeStyle.Is(lifestyle);
      }

      private static BasedOnDescriptor Configure(Predicate<Type> predicate, Action<ComponentRegistration> configurer)
      {
        return Types
          .FromThisAssembly()
          .Where(predicate)
          .Configure(configurer);
      }

      private static bool IsViewModel(Type type)
      {
        return type.Name == "ViewModel";
      }

      private static bool IsViewModelFactory(Type type)
      {
        return type.Name == "IFactory" && type.IsInterface && type.Namespace.StartsWith("Grove.Ui");
      }

      private static void RegisterCardsSources(IWindsorContainer container)
      {
        container.Register(Classes.FromThisAssembly()
          .BasedOn(typeof (CardsSource))
          .WithService.Base()
          .LifestyleTransient());
      }

      private void RegisterConfiguration(IWindsorContainer container)
      {
        var registration = Component(typeof (Grove.Configuration), lifestyle: LifestyleType.Singleton);
        container.Register(registration);
      }

      private void ImplementUiStuff(ComponentRegistration<object> registration)
      {
        if (_configuration != Configuration.Ui)
          return;

        registration.Proxy
          .AdditionalInterfaces(
            typeof (INotifyPropertyChanged),
            typeof (INotifyPropertyChangedRaiser),
            typeof (INotifyCollectionChanged),
            typeof (IClosable))
          .Interceptors(
            typeof (NotifyPropertyChangedInterceptor),
            typeof (ClosedInterceptor));

        if (registration.Implementation.Implements<IReceive>())
        {
          registration.OnCreate((kernel, instance) =>
            {
              var game = kernel.Resolve<Match>().Game;
              game.Subscribe(instance);

              var disposed = (IClosable) instance;
              disposed.Closed += delegate { game.Unsubscribe(instance); };
            });
        }
      }

      private void RegisterShell(IWindsorContainer container)
      {
        var registration = Castle.MicroKernel.Registration.Component
          .For(typeof (IShell), typeof (Shell))
          .ImplementedBy(typeof (Shell))
          .LifestyleSingleton();

        ImplementUiStuff(registration);
        container.Register(registration);
      }

      private void RegisterViewModels(IWindsorContainer container)
      {
        container.Register(
          Castle.MicroKernel.Registration.Component.For(typeof (NotifyPropertyChangedInterceptor))
            .ImplementedBy(typeof (NotifyPropertyChangedInterceptor))
            .LifeStyle.Is(LifestyleType.Transient)
          );

        container.Register(
          Castle.MicroKernel.Registration.Component.For(typeof (ClosedInterceptor))
            .ImplementedBy(typeof (ClosedInterceptor))
            .LifeStyle.Is(LifestyleType.Transient)
          );


        container.Register(Configure(IsViewModel, registration =>
          {
            registration.LifestyleTransient();
            
            // inject game into viewmodels
            registration.DynamicParameters((k, d) => 
              d["game"] = k.Resolve<Match>().Game);
            
            ImplementUiStuff(registration);
          }));

        container.Register(Configure(IsViewModelFactory, registration =>
          {
            registration.AsFactory();
            registration.LifestyleTransient();
          }));
      }
    }
  }
}