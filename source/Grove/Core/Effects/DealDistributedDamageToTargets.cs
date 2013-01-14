namespace Grove.Core.Effects
{
  using System.Collections.Generic;
  using Grove.Core.Targeting;
  using Modifiers;

  public class DealDistributedDamageToTargets : Effect
  {
    private readonly List<int> _damageDistribution = new List<int>();
    public Value Amount = 0;    

    public override int CalculatePlayerDamage(Player player)
    {
      for (int i = 0; i < Targets.Count; i++)
      {
        if (player == Targets[i] && IsValid(Targets[i]))
        {
          return _damageDistribution[i];
        }
      }
      return 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      for (int i = 0; i < Targets.Count; i++)
      {
        if (creature == Targets[i] && IsValid(Targets[i]))
        {
          return _damageDistribution[i];
        }
      }

      return 0;
    }

    protected override void DistributeDamage(IDamageDistributor damageDistributor)
    {
      _damageDistribution.AddRange(damageDistributor.DistributeDamage(Targets, Amount.GetValue(X)));
    }

    protected override void ResolveEffect()
    {
      for (int i = 0; i < Targets.Count; i++)
      {
        var damage = new Damage(
          source: Source.OwningCard,
          amount: _damageDistribution[i],
          isCombat: false,
          changeTracker: Game.ChangeTracker);

        if (IsValid(Targets[i]))
          Targets[i].DealDamage(damage);
      }
    }

    public override string ToString()
    {
      return GetType().Name;
    }
  }
}