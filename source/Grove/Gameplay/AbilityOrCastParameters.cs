namespace Grove.Gameplay
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.CostRules;
  using AI.RepetitionRules;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Grove.Gameplay.Effects;

  public class AbilityOrCastParameters
  {
    private readonly List<MachinePlayRule> _machineRules = new List<MachinePlayRule>();
    public int DistributeAmount;
    public EffectFactory Effect;
    public TargetSelector TargetSelector = new TargetSelector();
    public string Text;
    public bool HasTimingRules { get { return _machineRules.Any(x => x is TimingRule); } }

    public List<MachinePlayRule> GetMachineRules()
    {
      return _machineRules;
    }

    public void TimingRule(TimingRule rule)
    {
      _machineRules.Add(rule);
    }

    public void RepetitionRule(RepetitionRule rule)
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