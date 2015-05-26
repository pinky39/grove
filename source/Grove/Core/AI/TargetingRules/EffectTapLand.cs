namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectTapLand : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Where(x => !x.IsTapped)
        .OrderByDescending(x => x.Score);

      return Group(candidates, p.TotalMinTargetCount());
    }
  }
}