namespace Grove.Tests.Unit
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;
  using Castle.Core;
  using Castle.DynamicProxy;
  using Castle.Facilities.TypedFactory;
  using Castle.MicroKernel;
  using Castle.MicroKernel.ComponentActivator;
  using Castle.MicroKernel.Context;
  using Castle.MicroKernel.Lifestyle;
  using Castle.MicroKernel.ModelBuilder;
  using Castle.MicroKernel.Registration;
  using Castle.Windsor;
  using Grove.Infrastructure;
  using Xunit;

  public class CastleWindsorFacts
  {
    [Fact(
      Skip =
        "Always fails, http://stackoverflow.com/questions/5803645/when-does-windsor-container-trigger-ikernel-componentdestroyed-event-for-transien"
      )]
    public void ComponentDestroyedEvent()
    {
      var wasDestroyed = false;

      var container = new WindsorContainer()
        .Register(
          Component.For(typeof (Cat))
            .LifeStyle.Transient
            .OnCreate((k, instance) =>
              {
                k.ComponentDestroyed += (model, component) =>
                  {
                    if (component == instance)
                      wasDestroyed = true;
                  };
              }));


      var cat = container.Resolve<Cat>();
      container.Release(cat);

      Assert.True(wasDestroyed);
    }

    [Fact]
    public void CustomActivator()
    {
      var container = new WindsorContainer();
      container.Kernel.ComponentModelBuilder.AddContributor(new MyContributor());
      container.Register(Component.For(typeof (Cat)).LifeStyle.Is(LifestyleType.Transient));
      container.Register(Component.For(typeof (Kitten)).LifeStyle.Is(LifestyleType.Transient));
      container.Register(Component.For(typeof (AnimalsThatCanSayHello)));

      container.Resolve<Cat>();
      container.Resolve<Kitten>();

      Assert.Equal(1, container.Resolve<AnimalsThatCanSayHello>().Count);
    }

    [Fact]
    public void OnCreateFacility()
    {
      var container = new WindsorContainer();

      container.Register(
        Component.For(typeof (AnimalsThatCanSayHello)),
        Component.For(typeof (Kitten)).OnCreate(
          (k, i) =>
            {
              var animals = k.Resolve<AnimalsThatCanSayHello>();
              animals.Add(i);
            }).LifeStyle.Transient);

      container.Resolve<Kitten>();
      Assert.Equal(1, container.Resolve<AnimalsThatCanSayHello>().Count);
    }

    [Fact]
    public void RegisterInterceptorWithAditionalInterface()
    {
      var container = new WindsorContainer()
        .Register(Component.For(typeof (MyInterceptor)));


      foreach (var type in new[] {typeof (Cat), typeof (Kitten)})
      {
        var registration = Component.For(type)
          .LifeStyle.Transient;


        if (type.HasAttribute<CanSayHelloAttribute>())
        {
          registration.
            Proxy.AdditionalInterfaces(typeof (ICanSayHello))
            .Interceptors(typeof (MyInterceptor));
        }

        container.Register(registration);
      }


      var cat = container.Resolve<Cat>();
      var kitten = container.Resolve<Kitten>();
      for (var i = 0; i < 3; i++)
      {
        var sayHello = cat as ICanSayHello;
        sayHello.Hello();
        cat.Eat();
      }

      Assert.Null(kitten as ICanSayHello);
      Assert.Equal(3, MyInterceptor.EatCount);
      Assert.Equal(3, MyInterceptor.HelloCount);
    }

    [Fact]
    public void ScopedLifestyleAndMultithreading()
    {
      var container = new WindsorContainer()
        .AddFacility<TypedFactoryFacility>()
        .Register(Component.For(typeof (First.IFactory)).AsFactory())
        .Register(Component.For(typeof (First)).LifestyleTransient())
        .Register(Component.For(typeof (Second)).LifestyleTransient())
        .Register(Component.For(typeof (IAmScoped)).LifestyleScoped());


      using (container.BeginScope())
      {
        Second second1 = null;
        Second second2 = null;

        var task1 = new Task(() =>
          {
            second1 = container.Resolve<Second>();
            second2 = container.Resolve<Second>();

            second1.DoIt();
            second2.DoIt();

            Thread.Sleep(100);
          });

        task1.Start(TaskScheduler.Default);

        Task.WaitAll(task1);


        Assert.Same(second1.Result.Scoped, second2.Result.Scoped);
      }

      using (container.BeginScope())
      {
        Second second1 = null;
        Second second2 = null;

        var task1 = new Task(() =>
          {
            second1 = container.Resolve<Second>();
            second2 = container.Resolve<Second>();

            Thread.Sleep(100);
          });

        task1.Start(TaskScheduler.Default);

        Task.WaitAll(task1);

        second1.DoIt();
        second2.DoIt();

        Assert.Same(second1.Result.Scoped, second2.Result.Scoped);
      }
    }


    [Fact]
    public void SubResolvers()
    {
      var container = new WindsorContainer();
      container.Kernel.Resolver.AddSubResolver(new PlayerResolver(container.Kernel));
      container.Register(Component.For<TheGame>().LifeStyle.Is(LifestyleType.Transient));
      container.Register(Component.For<Dashboard>().LifeStyle.Is(LifestyleType.Transient));

      var game = container.Resolve<TheGame>();

      Assert.Equal("Franc", game.Player1Dashboard.Owner.Name);
      Assert.Equal("Micka", game.Player2Dashboard.Owner.Name);
    }

    public class AnimalsThatCanSayHello : IEnumerable<object>
    {
      private readonly List<object> _animals = new List<object>();

      public int Count { get { return _animals.Count; } }

      public IEnumerator<object> GetEnumerator()
      {
        return _animals.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return GetEnumerator();
      }

      public void Add(object animal)
      {
        _animals.Add(animal);
      }
    }

    [CanSayHello]
    public class Cat
    {
      public virtual void Eat() {}
    }

    public class Dashboard
    {
      public Player Owner { get; set; }
    }

    public class First
    {
      public First(int p, IAmScoped amScoped)
      {
        Scoped = amScoped;
      }

      public IAmScoped Scoped { get; set; }

      public interface IFactory
      {
        First Create(int p);
      }
    }

    public class IAmScoped {}

    public interface ICanSayHello
    {
      void Hello();
    }

    public class Kitten
    {
      public virtual void Eat() {}
    }

    public class MyActivator : DefaultComponentActivator
    {
      public MyActivator(ComponentModel model, IKernel kernel, ComponentInstanceDelegate onCreation,
        ComponentInstanceDelegate onDestruction)
        : base(model, kernel, onCreation, onDestruction) {}


      public override object Create(CreationContext context, Burden burden)
      {
        var component = base.Create(context, burden);

        var animals = Kernel.Resolve<AnimalsThatCanSayHello>();
        animals.Add(component);
        return component;
      }
    }

    public class MyContributor : IContributeComponentModelConstruction
    {
      public void ProcessModel(IKernel kernel, ComponentModel model)
      {
        if (model.Implementation.HasAttribute<CanSayHelloAttribute>())
        {
          model.CustomComponentActivator = typeof (MyActivator);
        }
      }
    }

    public class MyInterceptor : IInterceptor
    {
      public static int EatCount { get; set; }
      public static int HelloCount { get; set; }

      public void Intercept(IInvocation invocation)
      {
        if (invocation.Method.DeclaringType == typeof (ICanSayHello))
        {
          var memberName = invocation.Method.Name;

          if (memberName == "Hello")
            HelloCount++;

          return;
        }

        invocation.Proceed();

        EatCount++;
      }
    }

    public class Player
    {
      public string Name { get; set; }
    }

    public class PlayerResolver : ISubDependencyResolver
    {
      private readonly IKernel _kernel;
      private readonly Player _player1;
      private readonly Player _player2;


      public PlayerResolver(IKernel kernel)
      {
        _kernel = kernel;
        _player1 = new Player {Name = "Franc"};
        _player2 = new Player {Name = "Micka"};
      }

      public bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver,
        ComponentModel model, DependencyModel dependency)
      {
        return dependency.DependencyKey.ToLowerInvariant().Contains("player");
      }

      public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model,
        DependencyModel dependency)
      {
        if (dependency.DependencyKey.ToLowerInvariant().Contains("player1"))
        {
          return _kernel.Resolve(dependency.TargetType, new Arguments(new[] {_player1}));
        }

        if (dependency.DependencyKey.ToLowerInvariant().Contains("player2"))
        {
          return _kernel.Resolve(dependency.TargetType, new Arguments(new[] {_player2}));
        }

        return null;
      }
    }

    public class Second
    {
      private readonly First.IFactory _factory;

      public Second(First.IFactory factory)
      {
        _factory = factory;
      }

      public First Result { get; set; }

      public void DoIt()
      {
        Result = _factory.Create(1);
      }
    }

    public class TheGame
    {
      public Dashboard Player1Dashboard { get; set; }
      public Dashboard Player2Dashboard { get; set; }
    }
  }

  public class CanSayHelloAttribute : Attribute {}
}