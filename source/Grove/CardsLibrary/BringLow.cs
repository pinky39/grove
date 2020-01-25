namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;
  using System.Linq;

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
        p.Text = "{{3}}{{R}}: Bring Low deals 3 damage to target creature. If that creature has a +1/+1 counter on it, Bring Low deals 5 damage to it instead.";
        p.Effect = () => new DealDamageToTargets(
            P((e, _) => e.Target.Card().CountersCount(CounterType.PowerToughness) > 0 ? 5 : 3));

        p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
        p.TargetingRule(new EffectDealDamage(5));

        p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
      });
    }
  }
}
