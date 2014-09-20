namespace Grove.Decisions
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.UserInterface;

  public class AssignCombatDamage : Decision
  {
    private readonly Attacker _attacker;

    private AssignCombatDamage() {}

    public AssignCombatDamage(Player controller, Attacker attacker) : base(controller,
      () => new UiHandler(), () => new MachineHandler(), () => new MachineHandler(), () => new PlaybackHandler())
    {
      _attacker = attacker;
    }

    private abstract class Handler : DecisionHandler<AssignCombatDamage, DamageDistribution>
    {
      protected override bool ShouldExecuteQuery
      {
        get
        {
          if (D._attacker.BlockersCount == 0 || D._attacker.AssignsDamageAsThoughItWasntBlocked)
            return false;

          return D._attacker.BlockersCount > 1 || D._attacker.HasTrample;
        }
      }

      public override void ProcessResults()
      {
        D._attacker.DistributeDamageToBlockers(Result);
      }

      protected override void SetResultNoQuery()
      {
        if (D._attacker.BlockersCount == 0 || D._attacker.AssignsDamageAsThoughItWasntBlocked)
        {
          Result = new DamageDistribution();
        }
        else if (D._attacker.BlockersCount == 1 &&
          !(D._attacker.HasTrample || D._attacker.AssignsDamageAsThoughItWasntBlocked))
        {
          Result = new DamageDistribution();
          Result.Assign(D._attacker.Blockers.First(), D._attacker.Card.CalculateCombatDamageAmount());
        }
      }
    }

    private class MachineHandler : Handler
    {
      public MachineHandler()
      {
        Result = new DamageDistribution();
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

      private void AssignUnassignedDamage(List<Blocker> blockers, int damageLeft, DamageDistribution damageDistribution)
      {
        if (!(D._attacker.HasTrample || D._attacker.AssignsDamageAsThoughItWasntBlocked) && damageLeft > 0)
          damageDistribution.Assign(blockers[0], damageLeft);
      }

      private DamageDistribution DeathTouchScenario()
      {
        var damageDistribution = new DamageDistribution();

        var damageLeft = D._attacker.Card.CalculateCombatDamageAmount();
        var blockers = D._attacker.Blockers.OrderByDescending(x => x.Score).ToList();

        if (!D._attacker.AssignsDamageAsThoughItWasntBlocked)
        {
          foreach (var blocker in blockers)
          {
            if (damageLeft == 0)
              break;

            damageDistribution.Assign(blocker, 1);

            damageLeft--;
          }
        }

        AssignUnassignedDamage(blockers, damageLeft, damageDistribution);

        return damageDistribution;
      }

      private DamageDistribution DefaultScenario()
      {
        var damageDistribution = new DamageDistribution();

        var damageLeft = D._attacker.Card.CalculateCombatDamageAmount();
        var blockers = D._attacker.BlockersInDamageAssignmentOrder.ToList();

        foreach (var blocker in blockers)
        {
          if (damageLeft == 0)
            break;

          var amount = damageLeft > blocker.LifepointsLeft ? blocker.LifepointsLeft : damageLeft;
          damageDistribution.Assign(blocker, amount);

          damageLeft -= amount;
        }

        AssignUnassignedDamage(blockers, damageLeft, damageDistribution);

        return damageDistribution;
      }
    }

    private class PlaybackHandler : Handler
    {
      protected override bool ShouldExecuteQuery { get { return true; } }

      public override void SaveDecisionResults() {}

      protected override void ExecuteQuery()
      {
        Result = (DamageDistribution) Game.Recorder.LoadDecisionResult();
      }
    }

    private class UiHandler : Handler
    {
      protected override void ExecuteQuery()
      {
        var result = new DamageDistribution();

        var dialog = Ui.Dialogs.CombatDamage.Create(D._attacker, result);
        Ui.Shell.ShowModalDialog(dialog);

        Result = result;
      }
    }
  }
}