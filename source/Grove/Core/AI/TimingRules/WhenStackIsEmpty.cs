namespace Grove.AI.TimingRules
{
  public class WhenStackIsEmpty : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return Stack.IsEmpty;
    }
  }
}