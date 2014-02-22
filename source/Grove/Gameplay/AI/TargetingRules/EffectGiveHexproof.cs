namespace Grove.Gameplay.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectGiveHexproof : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = GetCandidatesForProtectionFromTopSpell(p)
        .OrderByDescending(x => x.Score);

      return Group(candidates, p.MinTargetCount());
    }
  }
}