namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class CostDiscardCard : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .OrderBy(x => ScoreCalculator.CalculateDiscardScore(x, Ai.IsSearchInProgress));

      return Group(candidates, p.MaxTargetCount(),
        add: (trg, trgs) => trgs.Cost.Add(trg));
    }
  }
}