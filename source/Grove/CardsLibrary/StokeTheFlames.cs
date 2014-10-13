namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class StokeTheFlames : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Stoke the Flames")
        .ManaCost("{2}{R}{R}")
        .Type("Instant")
        .Text("{Convoke} {I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}Stoke the Flames deals 4 damage to target creature or player.")
        .Cast(p =>
        {
          p.Effect = () => new DealDamageToTargets(4);
          p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
          p.TargetingRule(new EffectDealDamage(4));
          p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
        });
    }
  }
}
