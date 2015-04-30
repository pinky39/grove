namespace Grove.AI.TimingRules
{
  public class AfterOpponentDeclaresAttackers : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return IsAfterOpponentDeclaresAttackers(p.Controller);
    }
  }
}