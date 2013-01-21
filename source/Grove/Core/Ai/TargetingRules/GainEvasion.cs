namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class GainEvasion : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => x.IsAttacker)
        .OrderByDescending(x => x.Card().CalculateCombatDamage(allDamageSteps: true));

      return Group(candidates, p.MinTargetCount());
    }
  }
}