namespace Grove.AI.TimingRules
{
  public class AfterYouDeclareBlockers : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return IsAfterYouDeclareBlockers(p.Controller);
    }
  }
}