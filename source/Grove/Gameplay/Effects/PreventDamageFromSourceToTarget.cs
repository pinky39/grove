namespace Grove.Gameplay.Effects
{
  using Damage;
  using Modifiers;
  using Targeting;

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
          Target = DamageTarget,
          X = X
        };

      var modifier = new AddDamagePrevention(new PreventDamageFromSource(source))
        .Initialize(mp, Game);

      DamageTarget.AddModifier(modifier);
    }
  }
}