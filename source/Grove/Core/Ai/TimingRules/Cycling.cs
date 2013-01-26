namespace Grove.Core.Ai.TimingRules
{
  public class Cycling : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Turn.Step == Step.EndOfTurn && !p.Controller.IsActive;
    }
  }
}