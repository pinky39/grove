namespace Grove.Artifical.TimingRules
{
  public class WhenStackIsNotEmpty : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return !Stack.IsEmpty;
    }
  }
}