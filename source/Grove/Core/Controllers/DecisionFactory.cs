namespace Grove.Core.Controllers
{
  using System;
  using System.Collections.Generic;
  using Infrastructure;
  using Scenario;

  [Copyable]
  public class DecisionFactory
  {
    private readonly IDecisionFactory _factory;
    private readonly CachedMachineResolver _cachedMachineResolver;
    private readonly ScenarioDecisions _scenarioDecisions;
    private Game _game;

    private DecisionFactory() {}

    public DecisionFactory(ScenarioDecisions scenarioDecisions, IDecisionFactory factory)
    {
      _scenarioDecisions = scenarioDecisions;
      _cachedMachineResolver = new CachedMachineResolver(factory);
      _factory = factory;
    }

    public void Init(Game game)
    {
      _game = game;
      _scenarioDecisions.Init(_game);
    }

    public IDecision Create<TDecision>(Player controller, Action<TDecision> setParameters) where TDecision : IDecision
    {
      TDecision decision;

      if (_game.Search.InProgress || controller.IsComputer)
      {
        decision = _cachedMachineResolver.Resolve<TDecision>();
      }
      else if (controller.IsHuman)
      {
        decision = _factory.CreateHuman<TDecision>();
      }
      else
      {
        var scenariodecision = _scenarioDecisions.Get(setParameters);
        scenariodecision.Game = _game;
        scenariodecision.Controller = controller;
        scenariodecision.Init();        
        return scenariodecision;
      }

      decision.Game = _game;
      decision.Controller = controller;
      setParameters(decision);
      decision.Init();      
      return decision;
    }

    public void AddDecisions(IEnumerable<StepDecisions> decisions)
    {
      _scenarioDecisions.AddDecisions(decisions);
    }

    // resolving decisions via typed factories is slow
    // resolve machine decisions with typed factories only the first time
    // then use cached ctor
    private class CachedMachineResolver
    {
      private readonly Dictionary<Type, ParameterlessCtor> _cache = new Dictionary<Type, ParameterlessCtor>();
      private readonly IDecisionFactory _factory;
      private readonly object _lock = new object();

      public CachedMachineResolver(IDecisionFactory factory)
      {
        _factory = factory;
      }

      public TDecision Resolve<TDecision>()
      {
        lock (_lock)
        {
          ParameterlessCtor ctor;
          if (_cache.TryGetValue(typeof(TDecision), out ctor))
          {
            return (TDecision)ctor();
          }

          var decision = _factory.CreateMachine<TDecision>();
          _cache.Add(typeof(TDecision), decision.GetType().GetParameterlessCtor());
          return decision;  
        }        
      }
    }
  }
}