namespace Grove.Core.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Scenario;

  [Copyable]
  public class ScenarioDecisions
  {
    private readonly List<StepDecisions> _decisions = new List<StepDecisions>();
    private readonly IDecisionFactory _factory;
    private Game _game;

    private ScenarioDecisions() {}

    public ScenarioDecisions(IDecisionFactory factory)
    {
      _factory = factory;
    }

    public void AddDecisions(IEnumerable<StepDecisions> decisions)
    {
      _decisions.AddRange(decisions);
    }

    public void Init(Game game)
    {
      _game = game;
    }

    public IDecision Get<TDecision>(Action<TDecision> init) where TDecision : IDecision
    {
      if (typeof (TDecision) == typeof (PlaySpellOrAbility) ||
        typeof (TDecision) == typeof (DeclareAttackers) ||
          typeof (TDecision) == typeof (DeclareBlockers) ||
            typeof (TDecision) == typeof (SetTriggeredAbilityTarget))
      {
        var scenarioDecision = Next<TDecision>();

        if (scenarioDecision is Verify)
          return scenarioDecision;

        if (scenarioDecision == null)
          return new DefaultScenarioDecision();

        init((TDecision) scenarioDecision);
        return scenarioDecision;
      }

      var decision = _factory.CreateMachine<TDecision>();

      init(decision);
      return decision;
    }

    private IDecision Next<TDecision>()
    {
      var decisions = _decisions
        .Where(x => x.Step == _game.Turn.Step && x.Turn == _game.Turn.TurnCount)
        .SingleOrDefault();

      if (decisions == null)
        return null;

      return decisions.Next<TDecision>();
    }
  }
}