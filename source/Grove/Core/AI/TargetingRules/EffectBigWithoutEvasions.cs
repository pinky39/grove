namespace Grove.AI.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class EffectBigWithoutEvasions : TargetingRule
  {
    private readonly Func<Card, bool> _filter;

    private EffectBigWithoutEvasions() {}

    public EffectBigWithoutEvasions(Func<Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(attacker => _filter(attacker))
        .OrderByDescending(CalculateScore);

      return Group(candidates, p.TotalMinTargetCount());
    }

    private int CalculateScore(Card creature)
    {
      if (!creature.CanAttack)
        return -1;

      return Combat.CouldBeBlockedByAny(creature) ? 1 :
        creature.CalculateCombatDamageAmount(singleDamageStep: false);
    }
  }
}