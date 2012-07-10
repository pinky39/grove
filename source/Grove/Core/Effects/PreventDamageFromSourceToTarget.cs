namespace Grove.Core.Effects
{
  using Modifiers;
  using Preventions;

  public class PreventDamageFromSourceToTarget : Effect
  {
    private ITarget DamageSource { get { return Target(0); } }
    public bool OnlyOnce { get; set; }

    protected override void ResolveEffect()
    {
      var source = DamageSource.IsEffect()
        ? DamageSource.Effect().Source.OwningCard
        : DamageSource.Card();

      var prevention = new DamagePrevention.Factory<PreventDamageFromSource>
        {
          Game = Game,
          Init = (m, c) =>
            {
              m.Source = source;
              m.OnlyOnce = OnlyOnce;
            }
        };

      Modifier modifier = new Modifier.Factory<AddDamagePrevention>
        {
          Game = Game,
          Init = (m, _) => m.Prevention = prevention
        }
        .CreateModifier(Source.OwningCard, Target(1));


      Target(1).AddModifier(modifier);
    }
  }
}