namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;
  using System.Linq;

  public class EffectGiveDeathtouch : TargetingRule
  {
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = p.Candidates<Card>(ControlledBy.SpellOwner)        
        .Where(c => !c.Has().Deathtouch)
        .Where(c => c.Power > 0)
        .Where(c => c.CanAttack || c.CanBlock())
        .OrderBy(x => x.Power);

      return Group(candidates, p.TotalMinTargetCount());
    }
  }
}