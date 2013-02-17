namespace Grove.Core.Effects
{
  using Modifiers;
  using Preventions;
  using Targeting;

  public class PreventNextDamageFromSourceToController : Effect
  {
    protected override void ResolveEffect()
    {
      var source = Target.IsEffect()
        ? Target.Effect().Source.OwningCard
        : Target.Card();

      var prevention = new PreventDamageFromSource(source);
      var modifier = new AddDamagePrevention(prevention) {UntilEot = true};

      var mp = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          Target = Controller,
          X = X
        };

      modifier.Initialize(mp, Game);

      Controller.AddModifier(modifier);
    }
  }
}