namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class EffectGiveHexproof : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => Stack.CanBeDestroyedByTopSpell(x, targetOnly: true) || Stack.CanBeBouncedByTopSpell(x))
        .OrderByDescending(x => x.Score);

      return Group(candidates, p.MinTargetCount());
    }
  }
}