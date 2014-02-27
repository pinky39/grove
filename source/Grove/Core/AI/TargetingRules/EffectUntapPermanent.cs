namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectUntapPermanent : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)
        .Where(x => x.IsTapped && !x.Is().Land)
        .OrderByDescending(x => x.Score);        

      return Group(candidates, p.MinTargetCount());
    }
  }
}