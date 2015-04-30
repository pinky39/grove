namespace Grove.AI.TimingRules
{
  public class OnFirstDetachedOnSecondAttached : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      if (p.Card.IsAttached)
      {        
        return Turn.Step == Step.SecondMain;
      }
     
      return Turn.Step == Step.FirstMain;
    }
  }
}