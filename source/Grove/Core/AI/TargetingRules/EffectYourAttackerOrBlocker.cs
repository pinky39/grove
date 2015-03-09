namespace Grove.AI.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class EffectYourAttackerOrBlocker : TargetingRule
  {
    private readonly Func<Card, bool> _filter;

    public EffectYourAttackerOrBlocker(Func<Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    private EffectYourAttackerOrBlocker() {}

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(c => c.IsAttacker || c.IsBlocker)
        .Where(c => _filter(c))
        .OrderByDescending(c => c.CalculateCombatDamageAmount(singleDamageStep: false));

      return Group(candidates, p.MinTargetCount());
    }
  }
}