namespace Grove.Core.Ai.TimingRules
{
  public class Cycling : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return (Turn.Step == Step.FirstMain && p.Controller.IsActive) ||
        (Turn.Step == Step.EndOfTurn && !p.Controller.IsActive);
    }
  }
}