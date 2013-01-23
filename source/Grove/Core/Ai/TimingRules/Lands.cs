namespace Grove.Core.Ai.TimingRules
{
  public class Lands : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Turn.Step == Step.FirstMain;
    }
  }
}