namespace Grove.AI.TimingRules
{
  public class BeforeYouDeclareAttackers : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return IsBeforeYouDeclareAttackers(p.Controller);
    }
  }
}