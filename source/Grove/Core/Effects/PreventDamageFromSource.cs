namespace Grove.Effects
{
  using Modifiers;

  public class PreventDamageFromSource : Effect
  {
    private readonly bool _preventCombatOnly;

    private PreventDamageFromSource() {}

    public PreventDamageFromSource(bool preventCombatOnly)
    {
      _preventCombatOnly = preventCombatOnly;
    }

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
        source, _preventCombatOnly);

      var modifier = new AddDamagePrevention(prevention) {UntilEot = true};
      Game.AddModifier(modifier, mp);
    }
  }
}