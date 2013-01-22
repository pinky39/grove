namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class GainPacifism : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Select(x => new
          {
            Card = x,
            Score = CalculateAttackingPotencialScore(x)
          })
        .OrderByDescending(x => x.Score)
        .Select(x => x.Card)
        .ToList();

      return Group(candidates, p.MinTargetCount());
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>().OrderBy(x => x.Power);
      return Group(candidates, p.MinTargetCount());
    }
  }
}