namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class CombatDisabler : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Where(x => x.IsAbleToAttack || !x.Has().CannotBlock)
        .OrderByDescending(x => 2 * x.Power + x.Toughness);

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}