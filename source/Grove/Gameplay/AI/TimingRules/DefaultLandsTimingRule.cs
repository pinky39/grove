namespace Grove.Gameplay.AI.TimingRules
{
  public class DefaultLandsTimingRule : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return Turn.Step == Step.FirstMain;
    }
  }
}