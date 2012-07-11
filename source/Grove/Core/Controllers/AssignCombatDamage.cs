namespace Grove.Core.Controllers
{
  using System.Linq;
  using Details.Combat;
  using Results;

  public abstract class AssignCombatDamage : Decision<DamageDistribution>
  {
    public Attacker Attacker { get; set; }

    protected override bool ShouldExecuteQuery
    {
      get
      {
        if (Attacker.BlockersCount == 0)
          return false;

        return Attacker.BlockersCount > 1 || Attacker.HasTrample;
      }
    }

    public override void ProcessResults()
    {
      if (Attacker.BlockersCount == 0)
      {
        Result = new DamageDistribution();
      }
      else if (Attacker.BlockersCount == 1 && !Attacker.HasTrample)
      {
        Result = new DamageDistribution();
        Result.Assign(Attacker.Blockers.First(), Attacker.DamageThisWillDealInOneDamageStep);
      }

      Attacker.DistributeDamageToBlockers(Result);
    }
  }
}