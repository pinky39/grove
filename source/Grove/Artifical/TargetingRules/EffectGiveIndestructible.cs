namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Targeting;

  public class EffectGiveIndestructible : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = GetCandidatesThatCanBeDestroyed(p)
        .Where(x => !x.Has().Indestructible)
        .OrderByDescending(x => x.Card().Score);

      return Group(candidates, p.MinTargetCount());
    }
  }
}