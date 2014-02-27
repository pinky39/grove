namespace Grove.Effects
{
  using Modifiers;

  public class PreventNextDamageToTargets : Effect
  {
    private readonly int _amount;

    private PreventNextDamageToTargets() {}

    public PreventNextDamageToTargets(int amount)
    {
      _amount = amount;
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
        var prevention = new PreventNextDamageToTarget(_amount, target);
        var modifier = new AddDamagePrevention(prevention) {UntilEot = true};
        Game.AddModifier(modifier, mp);
      }
    }
  }
}