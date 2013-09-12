namespace Grove.Artifical.TimingRules
{
  public class WhenStackIsEmpty : TimingRule
  {
    public override bool? ShouldPlay2(TimingRuleParameters p)
    {
      return Stack.IsEmpty;
    }
  }
}