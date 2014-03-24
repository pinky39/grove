namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
    
  public class Shock : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Shock")
        .ManaCost("{R}")
        .Type("Instant")
        .Text("Shock deals 2 damage to target creature or player.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(2);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectDealDamage(2));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
          });
    }
  }
}