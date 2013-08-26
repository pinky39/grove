namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;    
  
  public class AfterOpponentDeclaresAttackers : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (p.Controller.IsActive)
        return false;

      return Turn.Step == Step.DeclareAttackers && Combat.AttackerCount > 0;
    }
  }
}