namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class GainPacifism : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Select(x => new
          {
            Card = x,
            Score = CalculateAttackingPotential(x)
          })
        .OrderByDescending(x => x.Score)
        .Select(x => x.Card)
        .ToList();

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>().OrderBy(x => x.Power);
      return Group(candidates, p.MinTargetCount());
    }
  }
}