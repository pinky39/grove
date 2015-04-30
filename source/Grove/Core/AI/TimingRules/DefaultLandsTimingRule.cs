namespace Grove.AI.TimingRules
{
  public class DefaultLandsTimingRule : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return true;
    }
  }
}