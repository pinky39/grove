namespace Grove.AI.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class CostDiscardCard : TargetingRule
  {
    private readonly Func<Card, int> _orderBy;

    private CostDiscardCard() {}

    public CostDiscardCard(Func<Card, int> orderBy = null)
    {
      _orderBy = orderBy ?? (x => ScoreCalculator.CalculateDiscardScore(x, Ai.IsSearchInProgress));
    }


    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .OrderBy(_orderBy);

      return Group(candidates, p.TotalMaxTargetCount(),
        add: (trg, trgs) => trgs.Cost.Add(trg));
    }
  }
}