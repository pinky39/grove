namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Effects;
  using Gameplay.Player;
  using Gameplay.Targeting;

  public class Counterspell : TargetingRule
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