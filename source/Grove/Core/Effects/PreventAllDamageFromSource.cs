namespace Grove.Effects
{
  using Modifiers;

  public class PreventAllDamageFromSource : Effect
  {
    private readonly bool _preventCombatOnly;

    private PreventAllDamageFromSource() {}

    public PreventAllDamageFromSource(bool preventCombatOnly)
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

      var prevention = new Grove.PreventDamageFromSource(
        source, _preventCombatOnly);

      var modifier = new AddDamagePrevention(prevention) {UntilEot = true};
      Game.AddModifier(modifier, mp);
    }
  }
}