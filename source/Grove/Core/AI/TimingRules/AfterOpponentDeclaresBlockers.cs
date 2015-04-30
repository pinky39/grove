namespace Grove.AI.TimingRules
{
  public class AfterOpponentDeclaresBlockers : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return IsAfterOpponentDeclaresBlockers(p.Controller);
    }
  }
}