namespace Grove.Effects
{
  using Modifiers;

  public class PreventFirstDamageFromSourceToController : Effect
  {
    protected override void ResolveEffect()
    {
      var source = Target.IsEffect()
        ? Target.Effect().Source.OwningCard
        : Target.Card();

      var prevention = new PreventFirstDamageFromSourceToCreatureOrPlayer(
        source: source,
        creatureOrPlayer: Controller);

      var modifier = new AddDamagePrevention(prevention) {UntilEot = true};

      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      Game.AddModifier(modifier, p);
    }
  }
}