namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectBounceOwnAndOpponents : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var ownCandidates = p.Candidates<Card>(selectorIndex: 0)
        .OrderBy(c => c.Score);

      var opponentCandidates = GetBounceCandidates(p, selectorIndex: 1);

      return Group(ownCandidates, opponentCandidates,
        (t, tgs) => tgs.AddEffect(t), (t, tgs) => tgs.AddEffect(t));
    }
  }
}