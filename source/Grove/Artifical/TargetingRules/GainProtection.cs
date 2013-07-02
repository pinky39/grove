namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.States;
  using Gameplay.Targeting;

  public class GainProtection : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => Stack.CanBeDestroyedByTopSpell(x, targetOnly: true) || Stack.CanBeBouncedByTopSpell(x))
        .OrderByDescending(x => x.Score)
        .ToList();

      if (Turn.Step == Step.BeginningOfCombat)
      {
        if (p.Controller.IsActive)
        {
          candidates.AddRange(
            p.Candidates<Card>(ControlledBy.SpellOwner)            
            .Where(x => x.CanAttack)
            .OrderByDescending(CalculateAttackerScoreForThisTurn));
        }
        else
        {
          candidates.AddRange(
            p.Candidates<Card>(ControlledBy.SpellOwner)
            .Where(x => x.CanBlock())
            .OrderByDescending(x => x.Power));
        }
      }

      return Group(candidates, p.MinTargetCount());
    }
  }
}