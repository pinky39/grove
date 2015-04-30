namespace Grove.AI.TimingRules
{
  public class OnFirstMain : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return p.Controller.IsActive && Turn.Step == Step.FirstMain && Stack.IsEmpty;
    }
  }
}