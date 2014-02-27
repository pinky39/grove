namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectCombatEquipment : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      IEnumerable<Card> candidates;

      if (Turn.Step == Step.FirstMain)
      {
        candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
          .Where(x => x.CanAttack)
          .Select(x => new
            {
              Card = x.Card(),
              Score = CalculateAttackerScoreForThisTurn(x)
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