namespace Grove.Core.Ai.TimingRules
{
  public class SecondMain : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Turn.Step == Step.SecondMain;
    }
  }
}