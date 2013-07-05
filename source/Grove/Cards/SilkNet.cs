namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class SilkNet : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Silk Net")
        .ManaCost("{G}")
        .Type("Instant")
        .Text("Target creature gets +1/+1 and gains reach until end of turn. (It can block creatures with flying.)")
        .FlavorText("Sometimes it's possible to pull a meal out of thin air.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToTargets(
              () => new AddPowerAndToughness(1, 1) {UntilEot = true},
              () => new AddStaticAbility(Static.Reach) {UntilEot = true})
              {Category = EffectCategories.ToughnessIncrease};

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            
            p.TimingRule(new Steps(activeTurn: false, passiveTurn: true, steps: Step.DeclareAttackers));
            p.TargetingRule(new GainReach());
          });
    }
  }
}