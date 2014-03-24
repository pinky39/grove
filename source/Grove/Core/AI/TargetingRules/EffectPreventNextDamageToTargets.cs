namespace Grove.AI.TargetingRules
{
  using System.Collections.Generic;

  public class EffectPreventNextDamageToTargets : TargetingRule
  {
    private readonly int _amount;
    
    private EffectPreventNextDamageToTargets() {}       
    public EffectPreventNextDamageToTargets(int amount = int.MaxValue)
    {
      _amount = amount;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = PreventNextDamage.GetCandidates(_amount, p, Game);      
      return Group(candidates, p.MinTargetCount(), p.MaxTargetCount());
    }
  }
}