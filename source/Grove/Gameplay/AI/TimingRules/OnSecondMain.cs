namespace Grove.Gameplay.AI.TimingRules
{
  public class OnSecondMain : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return p.Controller.IsActive && Turn.Step == Step.SecondMain && Stack.IsEmpty;
    }
  }
}