namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectGiveProtection : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = GetCandidatesForProtectionFromTopSpell(p)
        .OrderByDescending(x => x.Score)
        .ToList();

      if (IsBeforeYouDeclareAttackers(p.Controller))
      {
        candidates.AddRange(
          p.Candidates<Card>(ControlledBy.SpellOwner)
            .Where(x => x.CanAttack)
            .OrderByDescending(CalculateAttackerScoreForThisTurn));
      }
      else if (IsBeforeYouDeclareBlockers(p.Controller))
      {
        candidates.AddRange(
          p.Candidates<Card>(ControlledBy.SpellOwner)
            .Where(x => x.CanBlock())
            .OrderByDescending(x => x.Power));
      }

      return Group(candidates, p.MinTargetCount());
    }
  }
}