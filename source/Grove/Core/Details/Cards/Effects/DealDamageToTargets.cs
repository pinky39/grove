namespace Grove.Core.Details.Cards.Effects
{
  using System.Collections.Generic;
  using Modifiers;
  using Targeting;

  public class DealDamageToTargets : Effect
  {
    public Value Amount = 0;
    public bool GainLife;
    private readonly List<int> _damageDistribution = new List<int>();

    public override int CalculatePlayerDamage(Player player)
    {
      return player == Target() ? Amount.GetValue(X) : 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return creature == Target() ? Amount.GetValue(X) : 0;
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
                
        Targets[i].DealDamage(damage);

        if (GainLife)
          Controller.Life += damage.Amount;
      }
    }

    public override string ToString()
    {
      return GetType().Name;
    }
  }
}