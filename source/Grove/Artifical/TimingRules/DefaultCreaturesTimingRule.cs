namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;

  public class DefaultCreaturesTimingRule : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      if (p.Card.Has().Haste)
        return Turn.Step == Step.FirstMain;
      
      if (p.Card.Power < 2 || p.Card.Has().Defender)
        return Turn.Step == Step.SecondMain;

      return true;
    }
  }
}