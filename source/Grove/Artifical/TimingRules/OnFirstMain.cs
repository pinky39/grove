namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;

  public class OnFirstMain : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return p.Controller.IsActive && Turn.Step == Step.FirstMain;
    }
  }
}