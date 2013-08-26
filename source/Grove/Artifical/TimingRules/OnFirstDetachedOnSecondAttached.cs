namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;

  public class OnFirstDetachedOnSecondAttached : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (p.Card.IsAttached)
      {        
        return Turn.Step == Step.SecondMain;
      }
     
      return Turn.Step == Step.FirstMain;
    }
  }
}