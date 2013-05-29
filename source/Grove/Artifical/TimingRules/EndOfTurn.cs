namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;

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