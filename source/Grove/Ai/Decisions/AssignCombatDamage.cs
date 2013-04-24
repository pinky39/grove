namespace Grove.Ai.Decisions
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Combat;
  using Gameplay.Decisions.Results;

  public class AssignCombatDamage : Gameplay.Decisions.AssignCombatDamage
  {
    protected override void ExecuteQuery()
    {
      if (Attacker.HasDeathTouch)
      {
        Result = DeathTouchScenario();
        return;
      }      

      Result = DefaultScenario();
    }

    private void AssignUnassignedDamage(List<Blocker> blockers, int damageLeft, DamageDistribution damageDistribution)
    {
      if (!(Attacker.HasTrample || Attacker.AssignsDamageAsThoughItWasntBlocked) && damageLeft > 0)
        damageDistribution.Assign(blockers[0], damageLeft);
    }

    private DamageDistribution DeathTouchScenario()
    {
      var damageDistribution = new DamageDistribution();

      var damageLeft = Attacker.DamageThisWillDealInOneDamageStep;
      var blockers = Attacker.Blockers.OrderByDescending(x => x.Score).ToList();

      if (!Attacker.AssignsDamageAsThoughItWasntBlocked)
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

      var damageLeft = Attacker.DamageThisWillDealInOneDamageStep;
      var blockers = Attacker.BlockersInDamageAssignmentOrder.ToList();

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
}