namespace Grove.Artifical.TimingRules
{
  using System;
  using System.Linq;
  using Gameplay.States;

  [Serializable]
  public class ControllerIsAttacked : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (p.Controller.IsActive)
        return false;

      if (Turn.Step != Step.DeclareAttackers)
        return false;

      return Combat.Attackers.Any();
    }
  }
}