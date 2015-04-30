namespace Grove.AI.TimingRules
{
  public class AfterYouDeclareBlockers : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return IsAfterYouDeclareBlockers(p.Controller);
    }
  }
}