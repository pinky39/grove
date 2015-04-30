namespace Grove.AI.TimingRules
{
  public class DefaultCreaturesTimingRule : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      if (p.Card.Has().Haste)
        return Turn.Step == Step.FirstMain;
      
      if (p.Card.Power < 2 || p.Card.Has().Defender)
        return Turn.Step == Step.SecondMain;

      return true;
    }
  }
}