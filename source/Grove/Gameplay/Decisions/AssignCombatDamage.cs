namespace Grove.Gameplay.Decisions
{
  using System;
  using System.Linq;
  using Results;

  [Serializable]
  public abstract class AssignCombatDamage : Decision<DamageDistribution>
  {
    public Attacker Attacker { get; set; }

    protected override bool ShouldExecuteQuery
    {
      get
      {
        if (Attacker.BlockersCount == 0 || Attacker.AssignsDamageAsThoughItWasntBlocked)
          return false;

        return Attacker.BlockersCount > 1 || Attacker.HasTrample;
      }
    }

    public override void ProcessResults()
    {
      if (Attacker.BlockersCount == 0 || Attacker.AssignsDamageAsThoughItWasntBlocked)
      {
        Result = new DamageDistribution();
      }
      else if (Attacker.BlockersCount == 1 && !(Attacker.HasTrample || Attacker.AssignsDamageAsThoughItWasntBlocked))
      {
        Result = new DamageDistribution();
        Result.Assign(Attacker.Blockers.First(), Attacker.DamageThisWillDealInOneDamageStep);
      }

      Attacker.DistributeDamageToBlockers(Result);
    }
  }
}