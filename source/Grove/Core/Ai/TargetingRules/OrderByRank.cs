namespace Grove.Core.Ai.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class OrderByRank : TargetingRule
  {
    public ControlledBy ControlledBy = ControlledBy.Any;        
    public Func<Card, int> Rank;
    public Func<Card, int> ForceRank;

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy)
        .OrderBy(x => Rank(x));

      return Group(candidates, p.MinTargetCount());
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      var rank = ForceRank ?? Rank;
      
      var candidates = p.Candidates<Card>()
        .OrderBy(rank);

      return Group(candidates, p.MinTargetCount());
    }
  }
}