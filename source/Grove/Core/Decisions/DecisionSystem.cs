namespace Grove.Core.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using Infrastructure;
  using Scenario;

  public class DecisionSystem
  {
    private static readonly HashSet<Type> ScenarioDecisions = new HashSet<Type>(GetScenarioDecisions());
    private readonly IDecisionFactory _decisionFactory;

    private readonly PrerecordedDecisions _prerecordedDecisions = new PrerecordedDecisions();

    public DecisionSystem(IDecisionFactory decisionFactory)
    {
      _decisionFactory = decisionFactory;
    }

    private static IEnumerable<Type> GetScenarioDecisions()
    {
      return Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(x => x.Implements<IDecision>())
        .Where(x => x.Namespace.EndsWith("Scenario"))
        .Select(x => x.BaseType);
    }

    public void AddScenarioDecisions(IEnumerable<DecisionsForOneStep> decisions)
    {
      _prerecordedDecisions.AddDecisions(decisions);
    }

    public IDecision Create<TDecision>(Player player, Action<TDecision> setParameters, Game game)
      where TDecision : class, IDecision
    {
      IDecision decision;

      var controller = player.Controller == ControllerType.Human && game.Search.InProgress
        ? ControllerType.Machine
        : player.Controller;

      if (controller == ControllerType.Scenario && ExistsScenarioDecision<TDecision>())
      {
        decision =
          _prerecordedDecisions.GetNext<TDecision>(game.Turn.TurnCount, game.Turn.Step) ??
            new NopScenarioDecision();
      }
      else
      {
        decision = controller == ControllerType.Human
          ? _decisionFactory.CreateHuman<TDecision>()
          : _decisionFactory.CreateMachine<TDecision>();
      }


      decision.Game = game;
      decision.Controller = player;
      SetParameters(decision, setParameters);
      decision.Init();

      return decision;
    }

    private static bool ExistsScenarioDecision<TDecision>()
    {
      return ScenarioDecisions.Contains(typeof (TDecision));
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
  }
}