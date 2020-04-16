namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectLandEnchantment : TargetingRule
  {
    private readonly ControlledBy _controlledBy;

    public EffectLandEnchantment(ControlledBy controlledBy)
    {
      _controlledBy = controlledBy;
    }

    private EffectLandEnchantment() {}

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(_controlledBy)
        .OrderBy(c => c.HasAttachments ? 1 : 0)
        .ThenBy(c => c.IsTapped ? 1 : 0);
        
      return Group(candidates, 1);
    }
  }
}