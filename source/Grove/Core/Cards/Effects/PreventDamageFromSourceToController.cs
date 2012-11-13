namespace Grove.Core.Cards.Effects
{
  using Grove.Core.Targeting;
  using Modifiers;
  using Preventions;

  public class PreventDamageFromSourceToController : Effect
  {
    private ITarget DamageSource { get { return Target(); } }
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
        .CreateModifier(Source.OwningCard, Controller, X, Game);


      Controller.AddModifier(modifier);
    }
  }
}