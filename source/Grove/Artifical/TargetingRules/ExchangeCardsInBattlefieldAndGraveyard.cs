namespace Grove.Artifical.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class ExchangeCardsInBattlefieldAndGraveyard : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var battlefieldCandidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .OrderBy(x => x.Score);        

      var graveyardCandidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .OrderBy(x => -x.Score);

      return Group(battlefieldCandidates, graveyardCandidates);
    }
  }
}