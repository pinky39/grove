namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Targeting;

  public class AttachTargetToSelf : TargetingRule
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