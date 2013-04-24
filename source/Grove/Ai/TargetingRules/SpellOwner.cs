namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

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