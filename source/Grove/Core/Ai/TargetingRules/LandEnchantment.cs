namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using Targeting;

  public class LandEnchantment : TargetingRule
  {
    private readonly ControlledBy _controlledBy;
    public LandEnchantment(ControlledBy controlledBy)
    {
      _controlledBy = controlledBy;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(_controlledBy);
      return Group(candidates, 1);
    }
  }
}