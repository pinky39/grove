namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Targeting;

  public class EffectAnyPlayer : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      return Group(p.Candidates<Player>(), p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}