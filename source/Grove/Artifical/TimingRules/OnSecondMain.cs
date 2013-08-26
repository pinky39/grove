namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;

  public class OnSecondMain : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return p.Controller.IsActive && Turn.Step == Step.SecondMain;
    }
  }
}