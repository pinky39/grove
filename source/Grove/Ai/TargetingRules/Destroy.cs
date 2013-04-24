namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class Destroy : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .OrderByDescending(x => x.Score);

      return Group(candidates,  p.MinTargetCount(),  p.MaxTargetCount());
    }
  }
}