namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class GainHexproof : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)        
        .Where(x => Stack.CanBeDestroyedByTopSpell(x, targetOnly: true) || Stack.CanBeBouncedByTopSpell(x))
        .OrderByDescending(x => x.Score);

      return Group(candidates, p.MinTargetCount());
    }
  }
}