namespace Grove.AI.TimingRules
{
  public class BeforeYouDeclareAttackers : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return IsBeforeYouDeclareAttackers(p.Controller);
    }
  }
}