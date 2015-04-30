namespace Grove.AI.TimingRules
{
  public class OnSecondMain : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return p.Controller.IsActive && Turn.Step == Step.SecondMain && Stack.IsEmpty;
    }
  }
}