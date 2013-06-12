namespace Grove.Gameplay.Effects
{
  using System.Linq;
  using Characteristics;

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
      foreach (var target in ValidEffectTargets)
      {
        Source.OwningCard.DealDamageTo(
          _amount.Value,
          (IDamageable) target, 
          isCombat: false);


        if (_gainLife)
          Controller.Life += _amount.Value;
      }
    }

    public override string ToString()
    {
      return GetType().Name;
    }
  }
}