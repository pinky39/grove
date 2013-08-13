namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;

  public class ModifyAttackersAndBlockers : TimingRule
  {
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (p.Controller.IsActive)
        return Turn.Step == Step.DeclareBlockers;

      return Turn.Step == Step.DeclareAttackers;
    }
  }
}