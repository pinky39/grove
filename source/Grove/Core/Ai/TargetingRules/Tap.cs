namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class Tap : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Where(x => !x.IsTapped)
        .OrderByDescending(x => x.CalculateCombatDamage(allDamageSteps: true))
        .ThenByDescending(x => x.Score);

      return Group(candidates, p.MinTargetCount());
    }
  }
}