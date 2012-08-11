namespace Grove.Core.Details.Cards.Effects
{
  using Modifiers;
  using Preventions;
  using Targeting;

  public class PreventDamageFromSourceToController : Effect
  {
    private ITarget DamageSource { get { return Target(); } }
    public bool OnlyOnce { get; set; }

    public override bool NeedsTargets { get { return true; } }

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
        .CreateModifier(Source.OwningCard, Controller, X);


      Controller.AddModifier(modifier);
    }
  }
}