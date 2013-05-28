namespace Grove.Artifical.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Targeting;

  [Serializable]
  public class SpellOwner : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Player>()
        .Where(x => x == p.Controller);

      return Group(candidates, 1);
    }
  }
}