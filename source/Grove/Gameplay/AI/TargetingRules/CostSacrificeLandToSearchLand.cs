namespace Grove.Gameplay.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class CostSacrificeLandToSearchLand : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var costTargets = p.Candidates<Card>(selectorIndex: 0, selector: c => c.Cost)
        .OrderBy(x => x.IsTapped ? 0 : 1)
        .ThenBy(x => x.Score)
        .Take(1);

      return Group(costTargets, p.MinTargetCount(), add: (t, trgs) => trgs.AddCost(t));
    }
  }
}