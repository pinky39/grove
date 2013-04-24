namespace Grove.Gameplay.Effects
{
  using System.Linq;
  using Card;
  using Damage;
  using Player;
  using Targeting;

  public class DealDamageToTargets : Effect
  {
    private readonly DynParam<int> _amount;
    private readonly bool _gainLife;

    private DealDamageToTargets() {}

    public DealDamageToTargets(DynParam<int> amount, bool gainLife = false)
    {
      _amount = amount;
      _gainLife = gainLife;

      RegisterDynamicParameters(amount);
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return Targets.Effect.Any(x => x == player) ? _amount.Value : 0;
    }

    public override int CalculateCreatureDamage(Card creature)
    {
      return Targets.Effect.Any(x => x == creature) ? _amount.Value : 0;
    }

    protected override void ResolveEffect()
    {
      foreach (var t in ValidEffectTargets)
      {
        var damage = new Damage(
          source: Source.OwningCard,
          amount: _amount.Value,
          isCombat: false,
          changeTracker: ChangeTracker);

        t.DealDamage(damage);

        if (_gainLife)
          Controller.Life += damage.Amount;
      }
    }

    public override string ToString()
    {
      return GetType().Name;
    }
  }
}