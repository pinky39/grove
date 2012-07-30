namespace Grove.Core.Details.Cards.Effects
{
  using Modifiers;
  using Preventions;
  using Targeting;

  public class PreventDamageFromSourceToTarget : Effect
  {
    private ITarget DamageSource { get { return Targets[0]; } }
    public bool OnlyOnce { get; set; }

    public override bool NeedsTargets { get { return true; } }

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

      var modifier = new Modifier.Factory<AddDamagePrevention>
        {
          Game = Game,
          Init = (m, _) => m.Prevention = prevention
        }
        .CreateModifier(Source.OwningCard, Targets[1]);


      Targets[1].AddModifier(modifier);
    }
  }
}