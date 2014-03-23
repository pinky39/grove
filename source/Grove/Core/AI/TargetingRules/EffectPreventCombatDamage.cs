namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectPreventCombatDamage : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      if (IsAfterOpponentDeclaresAttackers(p.Controller))
      {
        var attackerCandidates = p.Candidates<Card>(ControlledBy.Opponent)
          .Where(x => x.IsAttacker)
          .OrderByDescending(CalculateAttackerScoreForThisTurn);

        return Group(attackerCandidates, p.MinTargetCount(), p.MaxTargetCount());
      }

      if (IsAfterOpponentDeclaresBlockers(p.Controller))
      {

        var blockerCandidates = p.Candidates<Card>(ControlledBy.Opponent)
          .Where(x => x.IsBlocker)
          .OrderByDescending(x =>
            {
              var attacker = Combat.FindBlocker(x).Attacker;

              if (attacker == null)
                return 0;

              var blockers = attacker.Blockers.Select(b => b.Card);

              if (QuickCombat.CanAttackerBeDealtLeathalDamage(attacker, blockers))
              {
                return attacker.Card.Score;
              }

              return 0;
            }
          );

        return Group(blockerCandidates, p.MinTargetCount(), p.MaxTargetCount());
      }

      return None<Targets>();
    }
  }
}