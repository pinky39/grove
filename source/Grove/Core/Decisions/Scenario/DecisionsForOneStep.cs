namespace Grove.Core.Decisions.Scenario
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using Results;
  using Targeting;

  public class DecisionsForOneStep
  {
    private readonly List<IScenarioDecision> _decisions = new List<IScenarioDecision>();

    private readonly Game _game;

    public DecisionsForOneStep(Step step, int turn, Game game)
    {
      Step = step;
      Turn = turn;
      _game = game;
    }

    public Step Step { get; private set; }
    public int Turn { get; private set; }

    public DecisionsForOneStep Activate(Card card, Card target, Card costTarget = null,
      int? x = null, int abilityIndex = 0)
    {
      return Activate(card, (ITarget) target, costTarget, x, abilityIndex);
    }

    public DecisionsForOneStep Activate(Action<ScenarioActivation> init)
    {
      var activation = new ScenarioActivation();
      init(activation);

      var decision = new PlaySpellOrAbility
        {
          Result = new ChosenPlayable
            {
              Playable = new ScenarioPlayableAbility(
                activation.Card,
                new ActivationParameters
                  {
                    Targets = activation.GetTargets(),
                    X = activation.X
                  },
                activation.Index)
            }
        };

      decision.Game = _game;
      decision.Controller = activation.Card.Controller;

      _decisions.Add(decision);
      return this;
    }

    public DecisionsForOneStep Untap()
    {
      var decision = new ChooseToUntap
        {
          Result = true
        };

      _decisions.Add(decision);
      return this;
    }

    public DecisionsForOneStep DoNotUntap()
    {
      var decision = new ChooseToUntap
        {
          Result = false
        };

      _decisions.Add(decision);
      return this;
    }

    public DecisionsForOneStep Activate(Card card, Player target, Card costTarget = null,
      int? x = null, int abilityIndex = 0)
    {
      return Activate(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.CostTargets(costTarget);
          p.X = x;
          p.Index = abilityIndex;
        });
    }

    public DecisionsForOneStep Activate(Card card, Card costTarget = null,
      int? x = null, int abilityIndex = 0)
    {
      return Activate(p =>
        {
          p.Card = card;
          p.CostTargets(costTarget);
          p.X = x;
          p.Index = abilityIndex;
        });
    }

    private DecisionsForOneStep Activate(Card card, ITarget target = null, ITarget costTarget = null,
      int? x = null, int abilityIndex = 0)
    {
      return Activate(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.CostTargets(costTarget);
          p.X = x;
          p.Index = abilityIndex;
        });
    }

    public void AssertAllWereExecuted()
    {
      foreach (var decision in _decisions)
      {
        if (!decision.HasCompleted)
          throw new InvalidOperationException(
            String.Format("Decision {0} was not executed.", decision));
      }
    }

    public DecisionsForOneStep Cast(Action<ScenarioActivation> init)
    {
      var activation = new ScenarioActivation();
      init(activation);

      var decision = new PlaySpellOrAbility
        {
          Result = new ChosenPlayable
            {
              Playable = new ScenarioPlayableSpell(
                activation.Card,
                new ActivationParameters
                  {
                    Targets = activation.GetTargets(),
                    X = activation.X
                  }, activation.Index)
            }
        };

      decision.Game = _game;
      decision.Controller = activation.Card.Controller;

      _decisions.Add(decision);
      return this;
    }

    public DecisionsForOneStep Cast(Card card, Card target, int index = 0, int? x = null)
    {
      return Cast(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.Index = index;
          p.X = x;
        });
    }

    public DecisionsForOneStep Cast(Card card, int index = 0, int? x = null)
    {
      return Cast(p =>
        {
          p.Card = card;
          p.Index = index;
          p.X = x;
        });
    }

    public DecisionsForOneStep Cast(Card card, Player target, int index = 0, int? x = null)
    {
      return Cast(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.Index = index;
          p.X = x;
        });
    }

    public DecisionsForOneStep Cast(Card card, ScenarioEffect target, int index = 0, int? x = null)
    {
      return Cast(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.Index = index;
          p.X = x;
        });
    }

    public DecisionsForOneStep DeclareAttackers(params Card[] attackers)
    {
      var decision = new DeclareAttackers
        {
          Result = attackers.ToList()
        };

      decision.Game = _game;
      decision.Controller = attackers[0].Controller;
      _decisions.Add(decision);
      return this;
    }

    public DecisionsForOneStep DeclareBlockers(params Card[] pairs)
    {
      Debug.Assert(pairs.Length%2 == 0, "Pairs lenght must be even number, did you forget to match a blocker?");

      var chosenBlockers = new ChosenBlockers();
      var defender = pairs[1].Controller;

      for (var i = 0; i < pairs.Length; i = i + 2)
      {
        var attacker = pairs[i];
        var blocker = pairs[i + 1];

        chosenBlockers.Add(blocker, attacker);
      }

      var decision = new DeclareBlockers
        {
          Result = chosenBlockers
        };

      _decisions.Add(decision);

      decision.Game = _game;
      decision.Controller = defender;

      return this;
    }

    public IDecision Next<TDecision>()
    {
      var first = _decisions.FirstOrDefault();

      if (first == null)
        return null;


      if ((first is TDecision || first is Verify) && first.CanExecute())
      {
        _decisions.Remove(first);
        return first;
      }

      return null;
    }

    public DecisionsForOneStep Target(Card target)
    {
      return Target((ITarget) target);
    }

    public DecisionsForOneStep Target(Player target)
    {
      return Target((ITarget) target);
    }

    private DecisionsForOneStep Target(ITarget target)
    {
      var decision = new SetTriggeredAbilityTarget
        {
          Result = new ChosenTargets(new Targets().AddEffect(target))
        };

      decision.Game = _game;
      _decisions.Add(decision);
      return this;
    }

    public DecisionsForOneStep Verify(Action assertion)
    {
      var decision = new Verify
        {
          Assertion = assertion,
        };

      decision.Game = _game;
      _decisions.Add(decision);
      return this;
    }

    public DecisionsForOneStep Cycle(Card card)
    {
      return Activate(p =>
        {
          p.Card = card;
          p.Index = 0;
        });
    }
  }
}