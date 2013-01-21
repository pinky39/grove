﻿namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Core.Targeting;

  public class CombatEquipment : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      IEnumerable<Card> candidates;

      if (Turn.Step == Step.FirstMain)
      {
        candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
          .Where(x => x.CanAttackThisTurn)
          .Select(x => new
            {
              Card = x.Card(),
              Score = CalculateAttackerScore(x)
            })
          .OrderByDescending(x => x.Score)
          .Where(x => x.Score > 0)
          .Select(x => x.Card);
      }
      else
      {
        candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
          .Where(x => x.CanBlock())
          .Select(x => new
            {
              Card = x,
              Score = CalculateBlockerScore(x)
            })
          .OrderByDescending(x => x.Score)
          .Where(x => x.Score > 0)
          .Select(x => x.Card);
      }

      return Group(candidates, 1);
    }
  }
}