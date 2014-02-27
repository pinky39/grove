namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectAttachToOwningCard : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .Where(x => x.IsGoodTarget(p.Card))
        .OrderByDescending(x => x.Score)
        .ToList();

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}