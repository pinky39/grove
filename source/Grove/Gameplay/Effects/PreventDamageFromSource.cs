namespace Grove.Gameplay.Effects
{
  using DamageHandling;
  using Modifiers;
  using Targeting;

  public class PreventDamageFromSource : Effect
  {
    protected override void ResolveEffect()
    {
      var source = Target.IsEffect()
        ? Target.Effect().Source.OwningCard
        : Target.Card();

      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this,
          X = X
        };

      var prevention = new PreventAllDamageFromSource(
        source: source);

      var modifier = new AddDamagePrevention(prevention) {UntilEot = true};      
      Game.AddModifier(modifier, mp);
    }
  }
}