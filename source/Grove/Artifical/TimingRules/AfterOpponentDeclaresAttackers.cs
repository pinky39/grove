namespace Grove.Artifical.TimingRules
{
  public class AfterOpponentDeclaresAttackers : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return IsAfterOpponentDeclaresAttackers(p.Controller);
    }
  }
}