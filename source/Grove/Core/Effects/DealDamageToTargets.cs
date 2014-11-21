namespace Grove.Effects
{
  using System.Linq;
  using AI;

  public class DealDamageToTargets : Effect
  {
    private readonly DynParam<int> _amount;
    private readonly bool _gainLife;
    private readonly DynParam<bool> _canBePrevented;

    private DealDamageToTargets()
    {
    }

    public DealDamageToTargets(DynParam<int> amount, bool gainLife = false, DynParam<bool> canBePrevented = null)
    {
      _amount = amount;
      _gainLife = gainLife;
      _canBePrevented = canBePrevented ?? true;

      RegisterDynamicParameters(amount, canBePrevented);
      SetTags(EffectTag.DealDamage);
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
          isCombat: false,
          canBePrevented: _canBePrevented.Value);


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