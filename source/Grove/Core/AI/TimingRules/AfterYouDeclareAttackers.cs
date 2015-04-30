namespace Grove.AI.TimingRules
{
  public  class AfterYouDeclareAttackers : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return IsAfterYouDeclareAttackers(p.Controller) && Combat.AttackerCount > 0;
    }
  }
}