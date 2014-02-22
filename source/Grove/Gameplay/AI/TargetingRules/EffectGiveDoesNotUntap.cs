namespace Grove.Gameplay.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectGiveDoesNotUntap : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.Opponent)
        .Select(x => new
          {
            Card = x,
            Score =  x.Has().DoesNotUntap ? 0 : CalculateAttackingPotential(x)
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