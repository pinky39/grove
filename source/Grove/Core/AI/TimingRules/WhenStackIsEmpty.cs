namespace Grove.AI.TimingRules
{
  public class WhenStackIsEmpty : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return Stack.IsEmpty;
    }
  }
}