namespace Grove.Core.Ai.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class OrderByRank : TargetingRule
  {
    private readonly ControlledBy _controlledBy = ControlledBy.Any;
    private readonly Func<Card, int> _forceRank;
    private readonly Func<Card, int> _rank;

    private OrderByRank() {}

    public OrderByRank(Func<Card, int> rank, ControlledBy controlledBy = ControlledBy.Any,
      Func<Card, int> forceRank = null)
    {
      _rank = rank;
      _forceRank = forceRank ?? _rank;
      _controlledBy = controlledBy;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(_controlledBy)
        .OrderBy(x => _rank(x));

      return Group(candidates, p.MaxTargetCount());
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .OrderBy(_forceRank);

      return Group(candidates, p.MinTargetCount());
    }
  }
}