namespace Grove.Core.Controllers.Scenario
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using Details.Cards;
  using Results;
  using Targeting;
  using Zones;

  public class ScenarioActivation
  {
    private readonly Targets _targets = new Targets();

    public Card Card { get; set; }
    public int? X { get; set; }
    public int AbilityIndex { get; set; }
    public bool PayKicker { get; set; }

    public Targets GetTargets()
    {
      return _targets;
    }

    public void Targets(params ITarget[] effectTargets)
    {
      foreach (var effectTarget in effectTargets)
      {
        if (effectTarget != null)
          _targets.AddEffect(effectTarget);
      }
    }

    public void CostTargets(params ITarget[] costTargets)
    {
      foreach (var costTarget in costTargets)
      {
        if (costTarget != null)
          _targets.AddCost(costTarget);
      }
    }
  }

  public class StepDecisions
  {
    private readonly List<IScenarioDecision> _decisions = new List<IScenarioDecision>();

    public Step Step { get; set; }
    public int Turn { get; set; }
    public Game Game { get; set; }

    public StepDecisions Activate(Card card, Card target, Card costTarget = null,
                                  int? x = null, int abilityIndex = 0)
    {
      return Activate(card, (ITarget) target, costTarget, x, abilityIndex);
    }

    public StepDecisions Activate(Action<ScenarioActivation> init)
    {
      var activation = new ScenarioActivation();
      init(activation);

      _decisions.Add(new PlaySpellOrAbility
        {
          Game = Game,
          Controller = activation.Card.Controller,
          Result = new ChosenPlayable
            {
              Playable = new ScenarioAbility(
                activation.Card,
                new ActivationParameters
                  (
                  targets: activation.GetTargets(),
                  x: activation.X
                  ),
                activation.AbilityIndex
                )
            }
        });

      return this;
    }

    public StepDecisions Activate(Card card, Player target, Card costTarget = null,
                                  int? x = null, int abilityIndex = 0)
    {
      return Activate(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.CostTargets(costTarget);
          p.X = x;
          p.AbilityIndex = abilityIndex;
        });
    }

    public StepDecisions Activate(Card card, Card costTarget = null,
                                  int? x = null, int abilityIndex = 0)
    {
      return Activate(p =>
        {
          p.Card = card;
          p.CostTargets(costTarget);
          p.X = x;
          p.AbilityIndex = abilityIndex;
        });
    }

    private StepDecisions Activate(Card card, ITarget target = null, ITarget costTarget = null,
                                   int? x = null, int abilityIndex = 0)
    {
      return Activate(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.CostTargets(costTarget);
          p.X = x;
          p.AbilityIndex = abilityIndex;
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

    public StepDecisions Cast(Action<ScenarioActivation> init)
    {
      var activation = new ScenarioActivation();
      init(activation);

      _decisions.Add(new PlaySpellOrAbility
        {
          Game = Game,
          Controller = activation.Card.Controller,
          Result = new ChosenPlayable
            {
              Playable = new ScenarioSpell(
                activation.Card,
                new ActivationParameters
                  (
                  targets: activation.GetTargets(),
                  payKicker: activation.PayKicker,
                  x: activation.X
                  ))
            }
        });

      return this;
    }

    public StepDecisions Cast(Card card, Card target, bool payKicker = false, int? x = null)
    {
      return Cast(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.PayKicker = payKicker;
          p.X = x;
        });
    }

    public StepDecisions Cast(Card card, bool payKicker = false, int? x = null)
    {
      return Cast(p =>
        {
          p.Card = card;
          p.PayKicker = payKicker;
          p.X = x;
        });
    }

    public StepDecisions Cast(Card card, Player target, bool payKicker = false, int? x = null)
    {
      return Cast(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.PayKicker = payKicker;
          p.X = x;
        });
    }

    public StepDecisions Cast(Card card, LazyEffect target, bool payKicker = false, int? x = null)
    {
      return Cast(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.PayKicker = payKicker;
          p.X = x;
        });
    }

    public StepDecisions DeclareAttackers(params Card[] attackers)
    {
      _decisions.Add(new DeclareAttackers
        {
          Controller = attackers[0].Controller,
          Game = Game,
          Result = attackers.ToList()
        });
      return this;
    }

    public StepDecisions DeclareBlockers(params Card[] pairs)
    {
      Debug.Assert(pairs.Length%2 == 0, "Pairs lenght must be even number, did you forget to match a blocker?");

      var chosenBlockers = new ChosenBlockers();
      var defending = pairs[1].Controller;

      for (var i = 0; i < pairs.Length; i = i + 2)
      {
        var attacker = pairs[i];
        var blocker = pairs[i + 1];

        chosenBlockers.Add(blocker, attacker);
      }

      _decisions.Add(new DeclareBlockers
        {
          Game = Game,
          Controller = defending,
          Result = chosenBlockers
        });
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

    public StepDecisions Target(Card target)
    {
      return Target((ITarget) target);
    }

    public StepDecisions Target(Player target)
    {
      return Target((ITarget) target);
    }

    private StepDecisions Target(ITarget target)
    {
      _decisions.Add(new SetTriggeredAbilityTarget
        {
          Game = Game,          
          Result = new ChosenTargets(new Targets().AddEffect(target))
        });
      return this;
    }

    public StepDecisions Verify(Action assertion)
    {
      _decisions.Add(new Verify
        {          
          Assertion = assertion,
          Game = Game
        });

      return this;
    }

    public StepDecisions Cycle(Card card)
    {
      _decisions.Add(new PlaySpellOrAbility
        {
          Game = Game,
          Controller = card.Controller,
          Result = new ChosenPlayable
            {
              Playable = new ScenarioCyclable(
                card)
            }
        });

      return this;
    }
  }
}