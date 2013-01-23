namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using Targeting;

  public class LandEnchantment : TargetingRule
  {
    public ControlledBy ControlledBy;

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy);
      return Group(candidates, 1);
    }
  }
}