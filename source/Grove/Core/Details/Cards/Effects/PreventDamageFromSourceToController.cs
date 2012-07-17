namespace Grove.Core.Details.Cards.Effects
{
  using Modifiers;
  using Preventions;
  using Targeting;

  public class PreventDamageFromSourceToController : Effect
  {
    private ITarget DamageSource { get { return Target(0); } }
    public bool OnlyOnce { get; set; }

    protected override void ResolveEffect()
    {
      var source = DamageSource.IsEffect()
        ? DamageSource.Effect().Source.OwningCard
        : DamageSource.Card();

      var prevention = new DamagePrevention.Factory<Preventions.PreventDamageFromSource>
        {
          Game = Game,
          Init = (m, c) =>
            {
              m.Source = source;
              m.OnlyOnce = OnlyOnce;
            }
        };

      var modifier = new Modifier.Factory<AddDamagePrevention>
        {
          Game = Game,
          Init = (m, _) => m.Prevention = prevention
        }
        .CreateModifier(Source.OwningCard, Controller);


      Controller.AddModifier(modifier);
    }
  }
}