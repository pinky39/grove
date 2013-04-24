namespace Grove.Core.Ai.TimingRules
{
  public class Creatures : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (p.Card.Power < 2 || p.Card.Has().Defender)
        return Turn.Step == Step.SecondMain;

      return true;
    }
  }
}