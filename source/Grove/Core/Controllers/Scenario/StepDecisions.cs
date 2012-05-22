namespace Grove.Core.Controllers.Scenario
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Linq;
  using Results;
  using Zones;

  public class StepDecisions
  {
    private readonly List<IScenarioDecision> _decisions = new List<IScenarioDecision>();

    public Step Step { get; set; }
    public int Turn { get; set; }
    public Stack Stack { get; set; }

    public StepDecisions Activate(Card card, Card target, Card costTarget = null,
      int? x = null, int abilityIndex = 0)
    {
      return Activate(card, (ITarget)target, costTarget, x, abilityIndex);
    }

    public StepDecisions Activate(Card card, Player target, Card costTarget = null,
      int? x = null, int abilityIndex = 0)
    {
      return Activate(card, (ITarget)target, costTarget, x, abilityIndex);
    }

    public StepDecisions Activate(Card card, Card costTarget = null,
      int? x = null, int abilityIndex = 0)
    {
      return Activate(card, (ITarget)null, costTarget, x, abilityIndex);
    }

    private StepDecisions Activate(Card card, ITarget target = null, ITarget costTarget = null,
      int? x = null, int abilityIndex = 0)
    {
      _decisions.Add(new PlaySpellOrAbility{
        Result = new ChosenPlayable{
          Playable = new ScenarioAbility(
            card,
            new ActivationParameters{
              CostTarget = costTarget,
              EffectTarget = target,
              X = x
            },
            abilityIndex
            )
        }
      });

      return this;
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

    public StepDecisions Cast(Card card, Card target, bool payKicker = false, int? x = null)
    {
      return Cast(card, (ITarget)target, payKicker, x);
    }

    public StepDecisions Cast(Card card, bool payKicker = false, int? x = null)
    {
      return Cast(card, (ITarget)null, payKicker, x);
    }

    public StepDecisions Cast(Card card, Player target, bool payKicker = false, int? x = null)
    {
      return Cast(card, (ITarget)target, payKicker, x);
    }

    public StepDecisions Cast(Card card, LazyEffect target, bool payKicker = false, int? x = null)
    {
      return Cast(card, (ITarget)target, payKicker, x);
    }

    private StepDecisions Cast(Card card, ITarget target, bool payKicker, int? x)
    {
      _decisions.Add(new PlaySpellOrAbility{
        Result = new ChosenPlayable{
          Playable = new ScenarioSpell(
            card,
            new ActivationParameters{
              EffectTarget = target,
              PayKicker = payKicker,
              X = x
            })
        }
      });

      return this;
    }

    public StepDecisions DeclareAttackers(params Card[] attackers)
    {
      _decisions.Add(new DeclareAttackers{
        Player = attackers[0].Controller,
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

      _decisions.Add(new DeclareBlockers{        
        Player = defending,
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
      _decisions.Add(new SetTriggeredAbilityTarget{
        Result = new ChosenTarget(target)
      });
      return this;
    }

    public StepDecisions Verify(Action assertion)
    {
      _decisions.Add(new Verify{
        Stack = Stack,
        Assertion = assertion
      });

      return this;
    }
  }
}