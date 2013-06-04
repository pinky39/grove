namespace Grove
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.ComponentModel;
  using System.Linq;
  using Artifical;
  using Castle.Core;
  using Castle.Facilities.TypedFactory;
  using Castle.MicroKernel;
  using Castle.MicroKernel.ModelBuilder;
  using Castle.MicroKernel.Registration;
  using Castle.MicroKernel.Resolvers;
  using Castle.MicroKernel.Resolvers.SpecializedResolvers;
  using Castle.MicroKernel.SubSystems.Configuration;
  using Castle.Windsor;
  using Gameplay;
  using Gameplay.Decisions;
  using Gameplay.Misc;
  using Gameplay.Tournaments;
  using Infrastructure;
  using UserInterface;
  using UserInterface.Decisions;
  using UserInterface.Permanent;
  using UserInterface.Shell;

  public class IoC
  {
    public enum Configuration
    {
      Ui,
      Test
    }

    private readonly Configuration _configuration;

    private IWindsorContainer _container;

    public IoC(Configuration configuration)
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
      var container = new WindsorContainer()
        .Install(new Registration(_configuration));

      Install(container);

      return container;
    }

    protected virtual void Install(IWindsorContainer container) {}

    private class Registration : IWindsorInstaller
    {
      private readonly Configuration _configuration;

      public Registration(Configuration configuration)
      {
        _configuration = configuration;
      }

      public void Install(IWindsorContainer container, IConfigurationStore store)
      {
        container.Kernel.ComponentModelBuilder.AddContributor(new RequireViewModelProperties());

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

          container.Register(Component(typeof (ViewModelFactories), lifestyle: LifestyleType.Singleton));
          container.Register(Component(typeof (Match)));
          container.Register(Component(typeof (Match.IFactory)).AsFactory());
          container.Register(Component(typeof (MatchRunner), lifestyle: LifestyleType.Singleton));
          container.Register(Component(typeof (Tournament)));
          container.Register(Component(typeof (Tournament.IFactory)).AsFactory());
          container.Register(Component(typeof (TournamentRunner), lifestyle: LifestyleType.Singleton));
          container.Register(Component(typeof (CombatMarkers), lifestyle: LifestyleType.Singleton));
          container.Register(Component(typeof (CardSelector)));
        }

        RegisterCardsSources(container);
        RegisterDecisions(container);

        container.Register(Component(typeof (Game)));
        container.Register(Component(typeof (Game.IFactory)).AsFactory());
        container.Register(Component(typeof (MatchSimulator), lifestyle: LifestyleType.Singleton));
        container.Register(Component(typeof (CardsDatabase), lifestyle: LifestyleType.Singleton));
        container.Register(Component(typeof (CardsInfo), lifestyle: LifestyleType.Singleton));
        container.Register(Component(typeof (DeckBuilder), lifestyle: LifestyleType.Singleton));
        container.Register(Component(typeof (DeckEvaluator), lifestyle: LifestyleType.Singleton));
      }

      public static ComponentRegistration<object> Component(Type service, Type implementation = null,
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

      private void RegisterDecisions(IWindsorContainer container)
      {
        container.Register(Component(typeof (DecisionSystem)));
        container.Register(Component(typeof (IDecisionFactory), lifestyle: LifestyleType.Singleton).AsFactory());

        container.Register(Classes.FromThisAssembly()
          .Where(x =>
            (
              x.Namespace.Equals(typeof (UserInterface.Decisions.TakeMulligan).Namespace)
              ) &&
                x.Implements<IDecision>())
          .WithServiceSelect((type, baseTypes) => new[] {type.BaseType})
          .LifestyleTransient());
      }

      private static BasedOnDescriptor Configure(Predicate<Type> predicate, Action<ComponentRegistration> configurer)
      {
        return Types
          .FromThisAssembly()
          .Where(predicate)
          .Configure(configurer);
      }

      public static bool IsViewModel(Type type)
      {
        return type.Name == "ViewModel";
      }

      private static bool IsViewModelFactory(Type type)
      {
        return type.Name == "IFactory" && type.IsInterface &&
          type.Namespace.StartsWith(typeof (ViewModelBase).Namespace);
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


        registration.OnCreate((kernel, instance) =>
          {
            if (registration.Implementation.Implements<IReceive>())
            {
              Game game = null;
              
              var match = kernel.Resolve<MatchRunner>().Current;
              var shell = kernel.Resolve<IShell>();
              
              if (match != null)
              {
                game = match.Game;
              }
              
              if (game != null) game.Subscribe(instance);

              shell.Subscribe(instance);

              var disposed = (IClosable) instance;
              disposed.Closed += delegate
                {
                  if (game != null) game.Unsubscribe(instance);
                  shell.Unsubscribe(instance);
                };
            }

            var initializable = instance as ViewModelBase;
            if (initializable != null)
            {
              initializable.Initialize();
            }
          });
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
            ImplementUiStuff(registration);
          }));

        container.Register(Configure(IsViewModelFactory, registration =>
          {
            registration.AsFactory();
            registration.LifestyleTransient();
          }));
      }
    }

    public class RequireViewModelProperties : IContributeComponentModelConstruction
    {
      public void ProcessModel(IKernel kernel, ComponentModel model)
      {
        foreach (var prop in model.Properties.Where(x => Registration.IsViewModel(x.Dependency.TargetType)))
        {
          prop.Dependency.IsOptional = false;
        }
      }
    }
  }
}