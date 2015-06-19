namespace Grove.Effects
{
  using Modifiers;

  public class PreventAllDamageToTargets : Effect
  {
    protected override void ResolveEffect()
    {
      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this,
        };

      foreach (var target in ValidEffectTargets)
      {
        var prevention = new PreventDamageToCreatureOrPlayer(target);
        var modifier = new AddDamagePrevention(prevention) {UntilEot = true};
        Game.AddModifier(modifier, mp);
      }
    }
  }
}