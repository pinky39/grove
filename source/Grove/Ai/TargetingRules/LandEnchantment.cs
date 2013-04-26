namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using Gameplay.Card;
  using Gameplay.Player;
  using Gameplay.Targeting;

  public class LandEnchantment : TargetingRule
  {
    private readonly ControlledBy _controlledBy;

    public LandEnchantment(ControlledBy controlledBy)
    {
      _controlledBy = controlledBy;
    }

    private LandEnchantment() {}

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(_controlledBy);
      return Group(candidates, 1);
    }
  }
}