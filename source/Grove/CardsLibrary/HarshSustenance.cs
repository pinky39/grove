namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class HarshSustenance : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Harsh Sustenance")
        .ManaCost("{1}{W}{B}")
        .Type("Instant")
        .Text("Harsh Sustenance deals X damage to target creature or player and you gain X life, where X is the number of creatures you control.")
        .FlavorText("The Shifting Wastes provide refuge to those who know where to look for it.")
        .Cast(p =>
        {
          p.Effect = () => new DealDamageToTargets(
              amount: P(e => e.Controller.Battlefield.Creatures.Count()),
              gainLife: true);

          p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

          p.TargetingRule(new EffectDealDamage(pt => pt.Controller.Battlefield.Creatures.Count()));
          p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
        });
    }
  }
}
