namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class CostSacrificeToRegenerate : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .OrderBy(x => x.Score);

      return Group(candidates, p.MinTargetCount(),
        add: (trg, trgs) => trgs.Cost.Add(trg));
    }
  }
}