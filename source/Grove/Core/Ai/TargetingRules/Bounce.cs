namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class Bounce : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = GetBounceCandidates(p);
      return Group(candidates, p.MinTargetCount());
    }

    private static IEnumerable<Card> GetBounceCandidates(TargetingRuleParameters p)
    {
      return p.Candidates<Card>(ControlledBy.Opponent)
        .Select(x => new
          {
            Card = x,
            Score = x.Owner == p.Controller ? 2*x.Score : x.Score
          })
        .OrderByDescending(x => x.Score)
        .Select(x => x.Card);
    }
  }
}