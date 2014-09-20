namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectCannotBlockAttack : TargetingRule
  {
    private readonly bool _attackOnly;
    private readonly bool _blockOnly;

    private EffectCannotBlockAttack() {}

    public EffectCannotBlockAttack(bool attackOnly = false, bool blockOnly = false)
    {
      _attackOnly = attackOnly;
      _blockOnly = blockOnly;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Where(x => (!_blockOnly && x.IsAbleToAttack) || (!_attackOnly && !x.Has().CannotBlock))
        .OrderByDescending(x => 2*x.Power + x.Toughness);

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}