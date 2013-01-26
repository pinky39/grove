namespace Grove.Core.Ai.TimingRules
{
  public class EndOfTurn : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (!p.Controller.IsActive && Turn.Step == Step.EndOfTurn)
        return true;

      return false;
    }
  }
}