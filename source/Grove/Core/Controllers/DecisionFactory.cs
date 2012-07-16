namespace Grove.Core.Controllers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using Scenario;

  [Copyable]
  public class DecisionFactory
  {
    private readonly List<StepDecisions> _decisions = new List<StepDecisions>();
    private readonly IDecisionFactory _factory;
    private Game _game;

    private DecisionFactory()
    {
      
    }
    
    public DecisionFactory(IDecisionFactory factory)
    {      
      _factory = factory;
    }

    public void Init(Game game)
    {
      _game = game;
    }

    public IDecision Create<TDecision>(Player controller, Action<TDecision> init) where TDecision : IDecision
    {
      TDecision decision;

      if (_game.Search.InProgress || controller.IsComputer)
      {
        var type = GetDecisionType<TDecision>(typeof (Machine.DeclareAttackers).Namespace);
        decision = (TDecision) _factory.Create(type);
      }
      else if (controller.IsHuman)
      {
        var type = GetDecisionType<TDecision>(typeof (Human.DeclareAttackers).Namespace);
        decision = (TDecision) _factory.Create(type);
      }
      else
      {
        var type = GetDecisionType<TDecision>(typeof (Scenario.DeclareAttackers).Namespace);      
        
        if (type == null)
        {
          // no special scenario decision, this will be handled by machine decision
          type = GetDecisionType<TDecision>(typeof (Machine.DeclareAttackers).Namespace);        
          decision = (TDecision) _factory.Create(type);
        }
        else
        {
          var scenarioDecision = Next<TDecision>();      
          
          if (scenarioDecision == null)
            return new DefaultScenarioDecision();
          
          if (scenarioDecision is Verify)
            return (IDecision)scenarioDecision;

          decision = (TDecision) scenarioDecision;
        }
      }

      decision.Init(_game, controller);
      init(decision);
      return decision;
    }   

    public void AddDecisions(IEnumerable<StepDecisions> decisions)
    {
      _decisions.AddRange(decisions);
    }

    private object Next<TDecision>()
    {
      var decisions = _decisions
        .Where(x => x.Step == _game.Turn.Step && x.Turn == _game.Turn.TurnCount)
        .SingleOrDefault();

      if (decisions == null)
        return null;

      return decisions.Next<TDecision>();
    }

    private static Type GetDecisionType<TDecision>(string @namespace)
    {
      return Type.GetType(String.Format("{0}.{1}",
        @namespace,
        typeof (TDecision).Name));
    }
  }
}