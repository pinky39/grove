namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class EffectGiveReach : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(c => c.CanBlock())
        .Where(c => !c.Has().Flying && !c.Has().Reach)
        .OrderByDescending(x => x.Power);

      return Group(candidates, p.MinTargetCount());
    }
  }
}