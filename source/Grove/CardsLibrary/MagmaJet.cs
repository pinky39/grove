namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class MagmaJet : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Magma Jet")
        .ManaCost("{1}{R}")
        .Type("Instant")
        .Text("Magma Jet deals 2 damage to target creature or player. Scry 2.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(new DealDamageToTargets(2), new Scry(2));
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectDealDamage(2));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
          });
    }
  }
}