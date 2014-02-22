namespace Grove.Gameplay.Decisions
{
  using System.Collections.Generic;
  using System.Linq;
  using Infrastructure;
  using UserInterface;

  public class SetDamageAssignmentOrder : Decision
  {
    private readonly Attacker _attacker;

    private SetDamageAssignmentOrder() {}

    public SetDamageAssignmentOrder(Player controller, Attacker attacker)
      : base(controller, () => new UiHandler(), () => new MachineHandler(), () => new MachineHandler(), () => new PlaybackHandler())
    {
      _attacker = attacker;
    }

    private abstract class Handler : DecisionHandler<SetDamageAssignmentOrder, DamageAssignmentOrder>
    {
      protected override bool ShouldExecuteQuery { get { return D._attacker.BlockersCount > 1; } }

      public override void ProcessResults()
      {
        D._attacker.SetDamageAssignmentOrder(Result);
      }

      protected override void SetResultNoQuery()
      {
        Result = new DamageAssignmentOrder();

        if (D._attacker.BlockersCount == 0)
          return;

        Result.Assign(D._attacker.Blockers.First(), 1);
      }
    }

    private class MachineHandler : Handler
    {
      public MachineHandler()
      {
        Result = new DamageAssignmentOrder();
      }

      protected override void ExecuteQuery()
      {
        if (D._attacker.HasDeathTouch)
        {
          Result = DeathTouchScenario();
          return;
        }

        Result = DefaultScenario();
      }

      private DamageAssignmentOrder DeathTouchScenario()
      {
        var damageAssignmentOrder = new DamageAssignmentOrder();

        var orderedByScore = D._attacker.Blockers
          .OrderByDescending(blocker => blocker.Score)
          .ToList();

        for (var i = 0; i < orderedByScore.Count; i++)
        {
          damageAssignmentOrder.Assign(orderedByScore[i], i);
        }

        return damageAssignmentOrder;
      }

      private DamageAssignmentOrder DefaultScenario()
      {
        var damageAssignmentOrder = new DamageAssignmentOrder();

        var blockers = GetBlockersThatCanBeDealtLeathalDamageProducingTheGreatestScore();
        blockers = IncludeOtherBlockersAfter(blockers);

        for (var i = 0; i < blockers.Count; i++)
        {
          damageAssignmentOrder.Assign(blockers[i], i);
        }

        return damageAssignmentOrder;
      }

      private List<Blocker> GetBlockersThatCanBeDealtLeathalDamageProducingTheGreatestScore()
      {
        var candidates = D._attacker.Blockers
          .Where(blocker => blocker.LifepointsLeft > 0)
          .Where(blocker => blocker.LifepointsLeft <= D._attacker.Card.CalculateCombatDamageAmount())
          .Select(blocker => new KnapsackItem<Blocker>(
            item: blocker,
            weight: blocker.LifepointsLeft,
            value: blocker.Score))
          .ToList();

        var result = Knapsack.Solve(candidates, D._attacker.Card.CalculateCombatDamageAmount());
        return result.OrderByDescending(x => x.Value).Select(x => x.Item).ToList();
      }

      private List<Blocker> IncludeOtherBlockersAfter(List<Blocker> blockers)
      {
        blockers.AddRange(
          D._attacker.Blockers
            .Where(blocker => !blockers.Contains(blocker))
            .OrderByDescending(blocker => blocker.Score)
          );

        return blockers;
      }
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (DamageAssignmentOrder) Game.Recorder.LoadDecisionResult();
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var result = new DamageAssignmentOrder();

        var dialog = Ui.Dialogs.DamageOrder.Create(D._attacker, result);
        Ui.Shell.ShowModalDialog(dialog);

        Result = result;
      }
    }
  }
}