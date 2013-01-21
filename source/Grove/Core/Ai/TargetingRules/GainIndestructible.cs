namespace Grove.Core.Ai.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Core.Targeting;

  public class GainIndestructible : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = GetCandidatesThatCanBeDestroyed(p)
        .OrderByDescending(x => x.Card().Score);

      return Group(candidates, p.MinTargetCount());
    }

    private IEnumerable<Card> GetCandidatesThatCanBeDestroyed(TargetingRuleParameters p)
    {
      return p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => Stack.CanBeDestroyedByTopSpell(x.Card()) || Combat.CanBeDealtLeathalCombatDamage(x.Card()));
    }
  }
}