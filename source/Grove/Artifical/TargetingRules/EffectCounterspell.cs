namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class EffectCounterspell : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      // top spell only
      var candidates = p.Candidates<Effect>(ControlledBy.Opponent)
        .Take(1);

      return Group(candidates, 1);
    }
  }
}