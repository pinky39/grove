namespace Grove.Gameplay.AI.TargetingRules
{
  using System.Collections.Generic;

  public class EffectAnyPlayer : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      return Group(p.Candidates<Player>(), p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}