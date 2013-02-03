namespace Grove.Core
{
  using System.Collections.Generic;
  using Ai;
  using Ai.CostRules;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Effects;
  using Targeting;

  public class AbilityOrCastParameters
  {
    public EffectFactory Effect;
    public TargetSelector TargetSelector = new TargetSelector();
    public CardText Text;    
     
    private readonly List<MachinePlayRule> _machineRules = new List<MachinePlayRule>();
     
    public List<MachinePlayRule> GetMachineRules()
    {
      return _machineRules;
    }

    public void TimingRule(TimingRule rule)
    {
      _machineRules.Add(rule);
    }

    public void TargetingRule(TargetingRule rule)
    {
      _machineRules.Add(rule);
    }

    public void CostRule(CostRule rule)
    {
      _machineRules.Add(rule);
    }
  }
}