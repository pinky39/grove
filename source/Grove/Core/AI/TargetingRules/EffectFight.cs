namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectFight : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var ownCandidates = p.Candidates<Card>(selectorIndex: 0)
        .OrderBy(c => -c.Toughness);

      var opponentCandidates = p.Candidates<Card>(selectorIndex: 1)
        .OrderBy(c => c.Toughness);

      return Group(ownCandidates, opponentCandidates,
        (t, tgs) => tgs.AddEffect(t), (t, tgs) => tgs.AddEffect(t));
    }
  }
}
