namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectPreventAttackerDamage : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .Where(x => x.IsAttacker)
        .OrderByDescending(CalculateAttackerScoreForThisTurn);

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}