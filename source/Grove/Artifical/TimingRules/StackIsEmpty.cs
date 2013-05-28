namespace Grove.Artifical.TimingRules
{
  using System;

  [Serializable]
  public class StackIsEmpty : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Stack.IsEmpty;
    }
  }
}