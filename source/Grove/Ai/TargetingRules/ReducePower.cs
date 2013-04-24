﻿namespace Grove.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Gameplay.Player;
  using Gameplay.Targeting;

  public class ReducePower : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {    
      var candidates = p.Candidates<Gameplay.Card.Card>(ControlledBy.Opponent)
        .Select(x => new
          {
            Card = x,
            Score = CalculateAttackingPotencialScore(x)
          })
        .Where(x => x.Score > 0)
        .OrderByDescending(x => x.Score)
        .Select(x => x.Card);


      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}