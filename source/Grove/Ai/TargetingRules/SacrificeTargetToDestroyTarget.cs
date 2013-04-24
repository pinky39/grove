﻿namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Card;
  using Gameplay.Player;
  using Gameplay.Targeting;

  public class SacrificeTargetToDestroyTarget : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var costTargets = p.Candidates<Card>(selectorIndex: 0, selector: c => c.Cost)
        .OrderBy(x => x.Score)
        .Take(1);

      var effectTargets = p.Candidates<Card>(selectorIndex: 0, selector: c => c.Effect,
        controlledBy: ControlledBy.Opponent)
        .OrderByDescending(x => x.Score);

      return Group(costTargets, effectTargets,
        add1: (t, trgs) => trgs.AddCost(t),
        add2: (t, trgs) => trgs.AddEffect(t));
    }
  }
}