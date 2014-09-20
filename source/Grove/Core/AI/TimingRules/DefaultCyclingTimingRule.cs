namespace Grove.AI.TimingRules
{
  public class DefaultCyclingTimingRule : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return (Turn.Step == Step.EndOfTurn && !p.Controller.IsActive && Stack.IsEmpty);
    }
  }
}