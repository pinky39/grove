namespace Grove.Core.Ai.TimingRules
{
  public class DeclareBlockers : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Turn.Step == Step.DeclareBlockers;
    }
  }
}