namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class CostSacrificeEffectGiveIndestructible : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var costCandidates = p.Candidates<Card>(selectorIndex: 0, selector: c => c.Cost)
        .OrderBy(x => x.Score)
        .Take(1);      

      var effectCandidates = GetCandidatesThatCanBeDestroyed(p, s => s.Effect)
        .Where(x => !x.Has().Indestructible)
        .OrderByDescending(x => x.Card().Score);

      return Group(costCandidates, effectCandidates,
        (t, tgs) => tgs.AddCost(t), (t, tgs) => tgs.AddEffect(t));
    }
  }
}
