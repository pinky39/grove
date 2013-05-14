namespace Grove.Artifical.TimingRules
{
  public class StackIsEmpty : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Stack.IsEmpty;
    }
  }
}