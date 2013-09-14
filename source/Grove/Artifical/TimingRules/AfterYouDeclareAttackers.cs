namespace Grove.Artifical.TimingRules
{
  public  class AfterYouDeclareAttackers : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return IsAfterYouDeclareAttackers(p.Controller) && Combat.AttackerCount > 0;
    }
  }
}