namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay.Card;
  using Gameplay.Player;
  using Gameplay.Targeting;

  public class TapLands : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Where(x => !x.IsTapped)
        .OrderByDescending(x => x.Score);

      return Group(candidates, p.MinTargetCount());
    }
  }
}