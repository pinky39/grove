namespace Grove.AI.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class EffectExileGraveyard : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .OrderByDescending(x => x.Score)
        .ToList();

      var pickedCount = Math.Min(p.TotalMaxTargetCount(), candidates.Count);

      return Group(candidates, pickedCount);
    }
  }
}