namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class CostSacrificeToGainLife : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      if (Stack.CanTopSpellReducePlayersLifeToZero(p.Controller))
      {
        var candidates1 = p.Candidates<Card>()
          .OrderBy(x => x.Score);

        return Group(candidates1, p.MinTargetCount(), add: (target, targets) => targets.Cost.Add(target));
      }

      var candidates = new List<Card>();

      if (Turn.Step == Step.DeclareBlockers && Stack.IsEmpty)
      {
        candidates.AddRange(
          p.Candidates<Card>()
            .Where(x => Combat.CanBeDealtLeathalCombatDamage(x))
            .Where(x => !Combat.CanKillAny(x)));
      }

      candidates.AddRange(
        p.Candidates<Card>()
          .Where(x => Stack.CanBeDestroyedByTopSpell(x)));

      return Group(candidates, p.MinTargetCount(), add: (target, targets) => targets.Cost.Add(target));
    }
  }
}