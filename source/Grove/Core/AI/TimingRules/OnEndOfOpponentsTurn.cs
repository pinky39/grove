namespace Grove.AI.TimingRules
{
  public class OnEndOfOpponentsTurn : TimingRule
  {
    public override bool ShouldPlayBeforeTargets(TimingRuleParameters p)
    {
      return IsEndOfOpponentsTurn(p.Controller);
    }
  }
}