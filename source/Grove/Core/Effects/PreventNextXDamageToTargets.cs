namespace Grove.Effects
{
  using Modifiers;

  public class PreventNextXDamageToTargets : Effect
  {
    private readonly DynParam<int> _amount;    
    
    private PreventNextXDamageToTargets() {}

    public PreventNextXDamageToTargets(DynParam<int> amount)
    {
      _amount = amount;

      RegisterDynamicParameters(amount);
    }

    protected override void ResolveEffect()
    {
      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this,
        };      

      foreach (var target in ValidEffectTargets)
      {
        var prevention = new PreventNextXDamageToCreatureOrPlayer(_amount.Value, target);
        var modifier = new AddDamagePrevention(prevention) {UntilEot = true};
        Game.AddModifier(modifier, mp);
      }
    }
  }
}