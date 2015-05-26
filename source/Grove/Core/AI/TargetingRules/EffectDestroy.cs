namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectDestroy : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .OrderByDescending(x => x.Score);

      return Group(candidates, p.TotalMinTargetCount(), p.TotalMaxTargetCount());
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .OrderBy(x => x.Score);

      return Group(candidates, p.TotalMinTargetCount(), p.TotalMaxTargetCount());
    }
  }
}