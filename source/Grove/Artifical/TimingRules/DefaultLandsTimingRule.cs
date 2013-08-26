namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;

  public class DefaultLandsTimingRule : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Turn.Step == Step.FirstMain;
    }
  }
}