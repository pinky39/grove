namespace Grove.Effects
{
  using Modifiers;

  public class PreventNextDamageToTargets : Effect
  {
    private readonly DynParam<int> _amount;
    private readonly bool _useAttachedToAsTarget;
    private readonly bool _eachTurn;

    private PreventNextDamageToTargets() {}

    public PreventNextDamageToTargets(DynParam<int> amount, bool useAttachedToAsTarget = false, bool eachTurn = false)
    {
      _amount = amount;
      _useAttachedToAsTarget = useAttachedToAsTarget;
      _eachTurn = eachTurn;

      RegisterDynamicParameters(amount);
    }

    protected override void ResolveEffect()
    {
      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this,
        };

      if (_useAttachedToAsTarget && Source.OwningCard.AttachedTo != null)
      {
        var prevention = new PreventNextDamageToTarget(_amount.Value, Source.OwningCard.AttachedTo, _eachTurn);
        var modifier = new AddDamagePrevention(prevention);
        Game.AddModifier(modifier, mp);
        
        return;
      }

      foreach (var target in ValidEffectTargets)
      {
        var prevention = new PreventNextDamageToTarget(_amount.Value, target);
        var modifier = new AddDamagePrevention(prevention) {UntilEot = true};
        Game.AddModifier(modifier, mp);
      }
    }
  }
}