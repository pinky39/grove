namespace Grove.Artifical.TimingRules
{
  using System;

  public class WhenStackIsEmpty : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Stack.IsEmpty;
    }
  }
}