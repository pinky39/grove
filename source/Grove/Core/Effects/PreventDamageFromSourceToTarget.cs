namespace Grove.Effects
{
  using Modifiers;

  public class PreventDamageFromSourceToTarget : Effect
  {
    private ITarget DamageSource { get { return Targets.Effect[0]; } }
    private ITarget DamageTarget { get { return Targets.Effect[1]; } }

    protected override void ResolveEffect()
    {
      var source = DamageSource.IsEffect()
        ? DamageSource.Effect().Source.OwningCard
        : DamageSource.Card();

      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this,
          X = X
        };

      var prevention = new PreventAllDamageFromSourceToTarget(
        source: source,
        creatureOrPlayer: DamageTarget,
        onlyOnce: true);

      var modifier = new AddDamagePrevention(prevention) {UntilEot = true};
      Game.AddModifier(modifier, mp);
    }
  }
}