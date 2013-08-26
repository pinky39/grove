namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class EffectUntapLand : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .OrderByDescending(x => x.IsTapped ? 1 : 0)
        .Select(x => x)
        .ToList();

      return Group(candidates, p.MinTargetCount());
    }
  }
}