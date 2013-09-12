namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;    
  
  public class AfterOpponentDeclaresAttackers : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      if (p.Controller.IsActive)
        return false;

      return Turn.Step == Step.DeclareAttackers && Combat.AttackerCount > 0;
    }
  }
}