namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;

  public class BeforeYouDeclareAttackers : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (Turn.Step == Step.BeginningOfCombat && p.Controller.IsActive)
      {
        return true;
      }

      return false;
    }
  }
}