namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical;
  using Artifical.CostRules;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class HeatRay : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Heat Ray")
        .ManaCost("{R}").HasXInCost()
        .Type("Instant")
        .Text("Heat Ray deals X damage to target creature.")
        .FlavorText("It's not known whether the Thran built the device to forge their wonders or to defend them.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(Value.PlusX);
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new EffectDealDamage());
            p.CostRule(new XIsTargetsLifepointsLeft());
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
          });
    }
  }
}