namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI;
  using AI.TargetingRules;
  using Effects;

  public class IncrementalGrowth : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Incremental Growth")
        .ManaCost("{3}{G}{G}")
        .Type("Sorcery")
        .Text("Put a +1/+1 counter on target creature, two +1/+1 counters on another target creature, and three +1/+1 counters on a third target creature.")
        .FlavorText("The bonds of family cross the boundaries of race.")
        .Cast(p =>
        {
          p.Effect = () => new PutIncrementalCountersOnTargets(1, 1)
            .SetTags(EffectTag.IncreasePower, EffectTag.IncreaseToughness);

          p.TargetSelector.AddEffect(trg =>
          {
            trg.MinCount = 3;
            trg.MaxCount = 3;
            trg.Is.Creature().On.Battlefield();
            trg.Message = "Select target creatures.";
          });

          p.TargetingRule(new EffectOrCostRankBy(c => c.Score));
        });
    }
  }
}
