namespace Grove.Decisions
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using Grove.Infrastructure;

  public class ScenarioStep
  {
    public readonly Step Step;
    public readonly int Turn;
    private readonly Game _game;
    private readonly List<DecisionResult> _results = new List<DecisionResult>();

    public ScenarioStep(Step step, int turn, Game game)
    {
      Step = step;
      Turn = turn;

      _game = game;
    }    

    public ScenarioStep Activate(Card card, Card target, Card costTarget = null,
      int? x = null, int abilityIndex = 0, bool stackShouldBeEmpty = true)
    {
      return Activate(card, (ITarget) target, costTarget, x, abilityIndex, stackShouldBeEmpty);
    }

    public ScenarioStep Activate(Action<ScenarioActivation> init)
    {
      var activation = new ScenarioActivation();
      init(activation);

      var result = new ChosenPlayable
        {
          Playable = new ScenarioPlayableAbility
            {
              Card = activation.Card,
              ActivationParameters = new ActivationParameters
                {
                  Targets = activation.GetTargets(),
                  X = activation.X
                },
              Index = activation.Index
            }
        };

      _results.Add(new DecisionResult(result, activation.Card.Controller, activation.StackShouldBeEmpty));
      return this;
    }

    public ScenarioStep Activate(Card card, ITarget[] targets)
    {
      return Activate(p =>
        {
          p.Card = card;
          p.Targets(targets);
        });
    }

    public ScenarioStep Activate(Card card, Player target, Card costTarget = null,
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

    public ScenarioStep Activate(Card card, Card costTarget = null,
      int? x = null, int abilityIndex = 0, bool stackShouldBeEmpty = true)
    {
      return Activate(p =>
        {
          p.Card = card;
          p.CostTargets(costTarget);
          p.X = x;
          p.Index = abilityIndex;
          p.StackShouldBeEmpty = stackShouldBeEmpty;
        });
    }

    public void AssertAllWereExecuted()
    {
      Asrt.True(_results.Count == 0,
        String.Format("All decisions of step {0} of turn {1} were not executed!", Step, Turn));
    }

    public ScenarioStep Cast(Action<ScenarioActivation> init)
    {
      var activation = new ScenarioActivation();
      init(activation);

      var result = new ChosenPlayable
        {
          Playable = new ScenarioPlayableSpell
            {
              Card = activation.Card,
              ActivationParameters = new ActivationParameters
                {
                  Targets = activation.GetTargets(),
                  X = activation.X
                },
              Index = activation.Index
            }
        };

      _results.Add(new DecisionResult(result, activation.Card.Controller, activation.StackShouldBeEmpty));
      return this;
    }

    public ScenarioStep Cast(Card card, Card target, int index = 0, int? x = null, bool stackShouldBeEmpty = true)
    {
      return Cast(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.Index = index;
          p.X = x;
          p.StackShouldBeEmpty = stackShouldBeEmpty;
        });
    }

    public ScenarioStep Cast(Card card, int index = 0, int? x = null, bool stackShouldBeEmpty = true)
    {
      return Cast(p =>
        {
          p.Card = card;
          p.Index = index;
          p.X = x;
          p.StackShouldBeEmpty = stackShouldBeEmpty;
        });
    }

    public ScenarioStep Cast(Card card, Player target, int index = 0, int? x = null, bool stackShouldBeEmpty = true)
    {
      return Cast(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.Index = index;
          p.X = x;
          p.StackShouldBeEmpty = stackShouldBeEmpty;
        });
    }

    public ScenarioStep Cast(Card card, ScenarioEffect target, int index = 0, int? x = null, bool stackShouldBeEmpty = true)
    {
      return Cast(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.Index = index;
          p.X = x;
          p.StackShouldBeEmpty = stackShouldBeEmpty;
        });
    }

    public ScenarioStep DeclareAttackers(params Card[] attackers)
    {
      _results.Add(new DecisionResult(new ChosenCards(attackers), null));
      return this;
    }

    public ScenarioStep DeclareBlockers(params Card[] pairs)
    {
      Debug.Assert(pairs.Length%2 == 0, "Pairs lenght must be even number, did you forget to match a blocker?");
      var chosenBlockers = new ChosenBlockers();

      for (var i = 0; i < pairs.Length; i = i + 2)
      {
        var attacker = pairs[i];
        var blocker = pairs[i + 1];

        chosenBlockers.Add(blocker, attacker);
      }

      _results.Add(new DecisionResult(chosenBlockers, null));
      return this;
    }

    public TResult PopNext<TResult>(Player controller) where TResult : class
    {
      if (_results.Count == 0)
      {
        return null;
      }

      var decisionResult = _results[0];

      if (decisionResult.Controller != null && decisionResult.Controller != controller)
        return null;

      if (!_game.Stack.IsEmpty && decisionResult.StackShouldBeEmpty)
        return null;

      var result = decisionResult.Result as TResult;
      if (result == null)
        return null;

      _results.Remove(decisionResult);
      return result;
    }

    public ScenarioStep NoValidTarget()
    {
      _results.Add(new DecisionResult(ChosenTargets.None()));
      return this;
    }

    public ScenarioStep Target(Card target)
    {
      return Target((ITarget) target);
    }

    public ScenarioStep Target(Player target)
    {
      return Target((ITarget) target);
    }

    public ScenarioStep Verify(Action assertion)
    {
      var verify = new Verify(assertion);
      _results.Add(new DecisionResult(verify));
      return this;
    }

    public ScenarioStep Cycle(Card card)
    {
      return Activate(p =>
        {
          p.Card = card;
          p.Index = 0;
        });
    }

    private ScenarioStep Activate(Card card, ITarget target = null, ITarget costTarget = null,
      int? x = null, int abilityIndex = 0, bool stackShouldBeEmpty = true)
    {
      return Activate(p =>
        {
          p.Card = card;
          p.Targets(target);
          p.CostTargets(costTarget);
          p.X = x;
          p.Index = abilityIndex;
          p.StackShouldBeEmpty = stackShouldBeEmpty;
        });
    }

    private ScenarioStep Target(ITarget target)
    {
      _results.Add(new DecisionResult(new ChosenTargets(new Targets().AddEffect(target))));
      return this;
    }

    private class DecisionResult
    {
      public readonly Player Controller;
      public readonly bool StackShouldBeEmpty;
      public readonly object Result;

      public DecisionResult(object result, Player controller = null, bool stackShouldBeEmpty = false)
      {
        Result = result;
        StackShouldBeEmpty = stackShouldBeEmpty;
        Controller = controller;
      }
    }
  }
}