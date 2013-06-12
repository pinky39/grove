namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Targeting;

  public class PreventDamageFromAttackers : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .Where(x => x.IsAttacker)
        .OrderByDescending(CalculateAttackerScore);

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}