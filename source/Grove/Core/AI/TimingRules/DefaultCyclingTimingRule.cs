namespace Grove.AI.TimingRules
{
  public class DefaultCyclingTimingRule : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return (Turn.Step == Step.EndOfTurn && !p.Controller.IsActive && Stack.IsEmpty);
    }
  }
}