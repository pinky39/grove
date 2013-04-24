namespace Grove.Core.Ai.TimingRules
{
  using System;

  public class Lands : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Turn.Step == Step.FirstMain;
    }
  }
}