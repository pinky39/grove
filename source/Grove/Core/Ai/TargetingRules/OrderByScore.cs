namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Core.Targeting;

  public class OrderByScore : TargetingRule
  {
    public ControlledBy ControlledBy = ControlledBy.Opponent;
    public bool Descending = true;

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy)
        .OrderByDescending(x => Descending ? x.Score : -x.Score);

      return Group(candidates, p.MinTargetCount());
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .OrderByDescending(x => Descending ? -x.Score : x.Score);

      return Group(candidates, p.MinTargetCount());
    }
  }
}