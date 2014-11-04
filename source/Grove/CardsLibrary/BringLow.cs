namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class BringLow : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bring Low")
        .ManaCost("{3}{R}")
        .Type("Instant")
        .Text("Bring Low deals 3 damage to target creature. If that creature has a +1/+1 counter on it, Bring Low deals 5 damage to it instead.")
        .FlavorText("\"People are often humbled by the elements. But the elements, too, can be humbled.\"{EOL}—Surrak, khan of the Temur")
        .Cast(p =>
        {
          p.Text = "{{3}}{{R}}: Bring Low deals 3 damage to target creature.";
          p.Effect = () => new DealDamageToTargets(3);
          p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

          p.TargetingRule(new EffectDealDamage(3));
          p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
        })
        .Cast(p =>
        {
          p.Text = "{{3}}{{R}}: Bring Low deals 5 damage to target creature with a +1/+1 counter on it.";
          p.Effect = () => new DealDamageToTargets(5);
          p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Creature && c.CountersCount(CounterType.PowerToughness) > 0).On.Battlefield());

          p.TargetingRule(new EffectDealDamage(5));
          p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
        });
    }
  }
}
