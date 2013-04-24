namespace Grove.Ai.TimingRules
{
  using Core;
  using Gameplay.States;

  public class Cycling : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return (Turn.Step == Step.EndOfTurn && !p.Controller.IsActive);
    }
  }
}