namespace Grove.AI.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class EffectOrCostRankBy : TargetingRule
  {
    private readonly ControlledBy _controlledBy = ControlledBy.Any;
    private readonly Func<Card, int> _forceRank;
    private readonly Func<Card, int> _rank;

    private EffectOrCostRankBy() {}

    public EffectOrCostRankBy(Func<Card, int> rank, ControlledBy controlledBy = ControlledBy.Any,
      Func<Card, int> forceRank = null)
    {
      _rank = rank;
      _forceRank = forceRank ?? _rank;
      _controlledBy = controlledBy;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(_controlledBy)
        .OrderBy(x => _rank(x))
        .ToList();

      if (p.HasEffectCandidates)
        return Group(candidates, p.TotalMinTargetCount(), p.TotalMaxTargetCount());

      return Group(candidates, p.TotalMinTargetCount(), p.TotalMaxTargetCount(), (trg, trgs) => trgs.AddCost(trg));
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .OrderBy(_forceRank);


      if (p.HasEffectCandidates)
        return Group(candidates, p.TotalMinTargetCount());

      return Group(candidates, p.TotalMinTargetCount(), add: (trg, trgs) => trgs.AddCost(trg));
    }
  }
}