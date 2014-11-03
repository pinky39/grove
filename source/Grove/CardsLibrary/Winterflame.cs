namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class Winterflame : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Winterflame")
        .ManaCost("{1}{U}{R}")
        .Type("Instant")
        .Text("Choose one or both —{EOL}• Tap target creature.{EOL}• Winterflame deals 2 damage to target creature.")
        .Cast(p =>
        {
          p.Text = "{{1}}{{U}}{{R}}: Tap target creature.";
          p.Effect = () => new TapTargets();
          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TargetingRule(new EffectTapCreature());
          p.TimingRule(new OnStep(Step.BeginningOfCombat));
        })
        .Cast(p =>
        {
          p.Text = "{{1}}{{U}}{{R}}: Winterflame deals 2 damage to target creature.";
          p.Effect = () => new DealDamageToTargets(2);
          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
          p.TargetingRule(new EffectDealDamage(2));
          p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
        })
        .Cast(p =>
        {
          p.Text = "{{1}}{{U}}{{R}}: Tap target creature. Winterflame deals 2 damage to target creature.";
          p.Effect = () => new DealDamageToAndTapTargets(2);
          p.TargetSelector.AddEffect(trg =>
          {
            trg.MinCount = 2;
            trg.MaxCount = 2;
            trg.Is.Creature().On.Battlefield();
          });
          // TODO: Add specific targeting rule
          p.TargetingRule(new EffectDealDamage(2));
          p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
        });
    }
  }
}
