namespace Grove.AI.TargetingRules
{
  using Grove.Infrastructure;
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
      if (p.EffectTargetTypeCount > 1)
      {
        return SelectTargets2Selectors(p);
      }

      var candidates = GetCandidates(p).ToList();

      if (p.DistributeAmount > 0)
      {
        return SelectTargetsDistribute(p, candidates);
      }

      if (p.HasEffectCandidates)
        return Group(candidates, p.TotalMinTargetCount(), p.TotalMaxTargetCount());

      return Group(candidates, p.TotalMinTargetCount(), p.TotalMaxTargetCount(), (trg, trgs) => trgs.AddCost(trg));
    }

    private IEnumerable<Card> GetCandidates(TargetingRuleParameters p, int selectorIndex = 0)
    {
      return p.Candidates<Card>(_controlledBy, selectorIndex: selectorIndex)
        .OrderBy(x => _rank(x));
    }

    private IEnumerable<Targets> SelectTargets2Selectors(TargetingRuleParameters p)
    {
      Asrt.True(p.EffectTargetTypeCount <= 2, "More than 2 effect selectors currently not supported.");

      var candidates1 = GetCandidates(p, 0).Cast<ITarget>().ToList();
      var candidates2 = GetCandidates(p, 1).Cast<ITarget>().ToList();

      return Group(candidates1, candidates2, minTargetCount1: p.MinTargetCount(selectorIndex: 0),
        minTargetCount2: p.MinTargetCount(selectorIndex: 1));
    }

    private IEnumerable<Targets> SelectTargetsDistribute(TargetingRuleParameters p, List<Card> candidates)
    {
      var minCount = p.TotalMinTargetCount();
      var maxCount = p.TotalMaxTargetCount();

      if (candidates.Count < minCount)
      {
        return None<Targets>();
      }

      var targetsCount = Math.Min(candidates.Count, maxCount);

      var targets = candidates
        .Take(targetsCount)
        .Cast<ITarget>()
        .ToList();

      var amount = p.DistributeAmount / targetsCount;
      var distribution = Enumerable.Range(amount, targetsCount).ToList();

      var reminder = p.DistributeAmount % targetsCount;

      for (int i = 0; i < reminder; i++)
      {
        distribution[i]++;
      }

      return Group(targets, distribution);
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .OrderBy(_forceRank)
        .ToList();

      if (p.DistributeAmount > 0)
      {
        return SelectTargetsDistribute(p, candidates);
      }

      if (p.HasEffectCandidates)
        return Group(candidates, p.TotalMinTargetCount());

      return Group(candidates, p.TotalMinTargetCount(), add: (trg, trgs) => trgs.AddCost(trg));
    }
  }
}