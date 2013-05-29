namespace Grove.Artifical.TimingRules
{
  using System;

  public class StackIsEmpty : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Stack.IsEmpty;
    }
  }
}