namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Card;
  using Gameplay.Targeting;

  public class Bounce : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = GetBounceCandidates(p);
      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .OrderBy(c => c.Score);

      return Group(candidates, p.MinTargetCount());
    }
  }
}