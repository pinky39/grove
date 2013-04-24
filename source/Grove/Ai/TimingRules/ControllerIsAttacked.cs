namespace Grove.Core.Ai.TimingRules
{
  using System.Linq;

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