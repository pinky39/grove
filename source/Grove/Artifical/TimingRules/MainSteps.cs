namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;

  public class MainSteps : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return (Turn.Step == Step.FirstMain || Turn.Step == Step.SecondMain) && p.Controller.IsActive;
    }
  }
}