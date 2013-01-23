namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class AttachTargetToSelf : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .Where(x => x.IsGoodTarget(p.Card))
        .OrderByDescending(x => x.Score)
        .ToList();

      return Group(candidates, 1);
    }
  }
}