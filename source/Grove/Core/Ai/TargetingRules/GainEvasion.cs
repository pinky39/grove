namespace Grove.Core.Ai.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class GainEvasion : TargetingRule
  {
    private readonly Func<Card, bool> _filter;

    private GainEvasion() {}

    public GainEvasion(Func<Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)        
        .Where(x => x.IsAttacker)
        .Where(x => _filter(x))
        .OrderByDescending(x => x.Card().CalculateCombatDamage(allDamageSteps: true));

      return Group(candidates, p.MinTargetCount());
    }
  }
}