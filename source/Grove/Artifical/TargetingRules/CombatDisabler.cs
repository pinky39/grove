namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class CombatDisabler : TargetingRule
  {
    private readonly bool _attackOnly;

    private CombatDisabler() {}

    public CombatDisabler(bool attackOnly = false)
    {
      _attackOnly = attackOnly;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Where(x => x.IsAbleToAttack || (!_attackOnly && !x.Has().CannotBlock))
        .OrderByDescending(x => 2*x.Power + x.Toughness);

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}