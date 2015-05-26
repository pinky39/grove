namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectGiveRegenerate : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = GetCandidatesThatCanBeDestroyed(p)
        .Where(x => !x.HasRegenerationShield)
        .OrderByDescending(x => x.Card().Score);

      return Group(candidates, p.TotalMinTargetCount());
    }
  }
}