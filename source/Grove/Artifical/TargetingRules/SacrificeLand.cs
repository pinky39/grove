namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Targeting;

  public class SacrificeLand : TargetingRule
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