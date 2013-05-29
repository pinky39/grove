namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;

  public class FirstMain : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Turn.Step == Step.FirstMain;
    }
  }
}