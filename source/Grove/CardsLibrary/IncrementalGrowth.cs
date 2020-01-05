namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
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
          var amounts = new[] { 1, 2, 3 };
          p.Effect = () => new PutDifferentAmountOf11ContersOnTargets(amounts);

          p.TargetSelector.AddEffect(
            trg => trg.Is.Creature().On.Battlefield(),
            trg => {
              trg.MinCount = 3;
              trg.MaxCount = 3;            
            });

          p.TargetingRule(new EffectOrCostRankBy(c => c.Score));
          p.TimingRule(new OnFirstMain());
        });
    }
  }
}
