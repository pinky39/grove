namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class LastDitchEffort : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Last-Ditch Effort")
        .ManaCost("{R}")
        .Type("Instant")
        .Text(
          "Sacrifice any number of creatures. Last-Ditch Effort deals that much damage to target creature or player.")
        .FlavorText("If you're gonna lose, at least make sure they don't win as much.")
        .Cast(p =>
          {
            p.Effect = () => new SacrificeCreaturesToDealDamageToTarget();
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectDealDamage(dp => dp.Controller.Battlefield.Creatures.Count()));

            p.TimingRule(new WhenYouHavePermanents(c => c.Is().Creature, minCount: 1));
            p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
          });
    }
  }
}