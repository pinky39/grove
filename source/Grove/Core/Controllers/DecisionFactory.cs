namespace Grove.Core.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using Infrastructure;
  using Scenario;

  [Copyable]
  public class DecisionFactory
  {
    private static readonly Dictionary<Type, Entry> CreateDecision = LoadEntries();
    private readonly Game _game;
    private PrerecordedDecisions _prerecordedDecisions = new PrerecordedDecisions();

    private DecisionFactory() {}

    public DecisionFactory(Game game)
    {
      _game = game;
    }

    private static Dictionary<Type, Entry> LoadEntries()
    {
      var entries = new Dictionary<Type, Entry>();

      var decisions = Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(type => type.Implements<IDecision>())        
        .ToList();

      foreach (var baseDecision in decisions.Where(type => type.Namespace.EndsWith("Controllers")))
      {
        entries[baseDecision] = new Entry();
      }

      foreach (var decision in decisions.Where(type => !type.IsAbstract && type.Namespace.EndsWith("Machine")))
      {
        var ctor = decision.GetParameterlessCtor();

        entries[decision.BaseType].CreateMachine = () => (IDecision) ctor();
        
        // for all scenario decisions that are not implemented use machine decisions
        entries[decision.BaseType].CreateScenario = () => (IDecision) ctor();
      }

      foreach (var decision in decisions.Where(type => !type.IsAbstract && type.Namespace.EndsWith("Human")))
      {
        var ctor = decision.GetParameterlessCtor();

        entries[decision.BaseType].CreateHuman = () => (IDecision) ctor();
      }

      foreach (var decision in decisions.Where(type => !type.IsAbstract && type.Namespace.EndsWith("Scenario")))
      {
        
        // default scenario decision is NOP.
        entries[decision.BaseType].CreateScenario = () => new NopScenarioDecision();
      }

      return entries;
    }

    public void AddScenarioDecisions(IEnumerable<DecisionsForOneStep> decisions)
    {
      _prerecordedDecisions.AddDecisions(decisions);
    }

    public IDecision Create<TDecision>(Player player, Action<TDecision> setParameters)
      where TDecision : class, IDecision
    {
      var controller = player.Controller == ControllerType.Human && _game.Search.InProgress 
          ? ControllerType.Machine
          : player.Controller;      

      var decision = CreateDecision[typeof (TDecision)][controller];

      if (controller == ControllerType.Scenario)
      {
        var nextScenarioDecision = _prerecordedDecisions.GetNext<TDecision>(_game.Turn.TurnCount, _game.Turn.Step) ??
          CreateDecision[typeof (TDecision)].CreateScenario();

        if (nextScenarioDecision != null)
          decision = nextScenarioDecision;
      }


      decision.Game = _game;
      decision.Controller = player;
      SetParameters(decision, setParameters);
      decision.Init();

      return decision;
    }

    private static void SetParameters<TDecision>(IDecision decision, Action<TDecision> setParameters)
      where TDecision : class
    {
      var settable = decision as TDecision;
      if (settable != null)
      {
        setParameters(settable);
      }
    }

    private class Entry
    {
      public Func<IDecision> CreateHuman;
      public Func<IDecision> CreateMachine;
      public Func<IDecision> CreateScenario;

      public IDecision this[ControllerType controllerType]
      {
        get
        {
          if (controllerType == ControllerType.Machine)
            return CreateMachine();

          if (controllerType == ControllerType.Human)
            return CreateHuman();

          return CreateScenario();
        }
      }
    }   
  }
}