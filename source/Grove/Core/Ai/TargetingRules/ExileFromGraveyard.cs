namespace Grove.Core.Ai.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class ExileFromGraveyard : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .OrderByDescending(x => x.Score)
        .ToList();

      var pickedCount = Math.Min(p.MaxTargetCount(), candidates.Count);

      return Group(candidates, pickedCount);
    }
  }
}