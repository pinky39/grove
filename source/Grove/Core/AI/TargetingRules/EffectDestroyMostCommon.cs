namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectDestroyMostCommon : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .GroupBy(x => x.Name)
        .OrderByDescending(x => x.Count())
        .SelectMany(x => x)
        .ToList();

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}