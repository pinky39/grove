namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectExchangeBattlefieldGraveyard : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var battlefieldCandidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .OrderBy(x => x.Score);        

      var graveyardCandidates = p.Candidates<Card>(ControlledBy.SpellOwner, selectorIndex: 1)
        .OrderBy(x => -x.Score);

      return Group(battlefieldCandidates, graveyardCandidates);
    }
  }
}