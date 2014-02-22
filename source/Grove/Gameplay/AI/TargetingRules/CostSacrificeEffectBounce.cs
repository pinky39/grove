namespace Grove.Gameplay.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class CostSacrificeEffectBounce : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var costCandidates = GetCandidatesThatCanBeDestroyed(p, s => s.Cost)
        .OrderBy(x => x.Score)
        .ToList();

      costCandidates.AddRange(
        p.Candidates<Card>(selector: s => s.Cost)
          .Where(x => x.Is().Land)
          .OrderBy(x => x.Score));

      var effectCandidates = GetBounceCandidates(p, s => s.Effect)
        .ToList();

      return Group(costCandidates, effectCandidates,
        (t, tgs) => tgs.AddCost(t), (t, tgs) => tgs.AddEffect(t));
    }
  }
}