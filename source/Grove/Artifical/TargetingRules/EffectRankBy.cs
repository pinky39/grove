namespace Grove.Artifical.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class EffectRankBy : TargetingRule
  {
    private readonly ControlledBy _controlledBy = ControlledBy.Any;
    private readonly Func<Card, int> _forceRank;
    private readonly Func<Card, int> _rank;

    private EffectRankBy() {}

    public EffectRankBy(Func<Card, int> rank, ControlledBy controlledBy = ControlledBy.Any,
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

      if (p.HasEffectCandidates)
        return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());

      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount(), (trg, trgs) => trgs.AddCost(trg));
    }

    protected override IEnumerable<Targets> ForceSelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>()
        .OrderBy(_forceRank);


      if (p.HasEffectCandidates)
        return Group(candidates, p.MinTargetCount());

      return Group(candidates, p.MinTargetCount(), add: (trg, trgs) => trgs.AddCost(trg));
    }
  }
}