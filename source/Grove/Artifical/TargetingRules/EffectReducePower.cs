namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class EffectReducePower : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Select(x => new
          {
            Card = x,
            Score = CalculateAttackingPotential(x)
          })
        .Where(x => x.Score > 0)
        .OrderByDescending(x => x.Score)
        .Select(x => x.Card);


      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}