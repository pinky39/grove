namespace Grove.Core.Ai.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using Targeting;

  public class SacrificeToDrawCards : TargetingRule
  {
    public Func<Card, bool> Filter = delegate { return true; };

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = new List<Card>();

      if (Turn.Step == Step.DeclareBlockers)
      {
        candidates.AddRange(
          p.Candidates<Card>()
            .Where(x => Filter(x))
            .Where(x => Combat.CanBeDealtLeathalCombatDamage(x))
            .Where(x => !Combat.CanKillAny(x)));
      }

      candidates.AddRange(
        p.Candidates<Card>()
          .Where(x => Filter(x))
          .Where(x => Stack.CanBeDestroyedByTopSpell(x)));

      return Group(candidates, p.MinTargetCount());
    }
  }
}