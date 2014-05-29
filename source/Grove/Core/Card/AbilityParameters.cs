namespace Grove
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.CostRules;
  using AI.RepetitionRules;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Infrastructure;

  [Copyable]
  public abstract class AbilityParameters
  {
    public readonly List<MachinePlayRule> Rules = new List<MachinePlayRule>();
    public int DistributeAmount;
    public EffectFactory Effect;
    public TargetSelector TargetSelector = new TargetSelector();
    public string Text;
    public bool UsesStack = true;

    public bool HasTimingRules { get { return Rules.Any(x => x is TimingRule); } }

    public void TimingRule(TimingRule rule) { Rules.Add(rule); }
    public void RepetitionRule(RepetitionRule rule) { Rules.Add(rule); }
    public void TargetingRule(TargetingRule rule) { Rules.Add(rule); }
    public void CostRule(CostRule rule) { Rules.Add(rule); }
  }
}