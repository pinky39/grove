namespace Grove.Core.Effects
{
  using Grove.Core.Targeting;
  using Modifiers;
  using Preventions;

  public class PreventDamageFromSourceToTarget : Effect
  {
    private ITarget DamageSource { get { return EffectTargets[0]; } }
    public bool OnlyOnce { get; set; }

    protected override void ResolveEffect()
    {
      var source = DamageSource.IsEffect()
        ? DamageSource.Effect().Source.OwningCard
        : DamageSource.Card();

      var prevention = Builder
        .Prevention<PreventDamageFromSource>(p =>
          {
            p.Source = source;
            p.OnlyOnce = OnlyOnce;
          });

      var modifier = Builder
        .Modifier<AddDamagePrevention>(m => m.Prevention = prevention)
        .CreateModifier(Source.OwningCard, EffectTargets[1], X, Game);

      EffectTargets[1].AddModifier(modifier);
    }
  }
}