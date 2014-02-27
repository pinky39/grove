namespace Grove.AI.TimingRules
{
  public class OnEndOfOpponentsTurn : TimingRule
  {
    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return IsEndOfOpponentsTurn(p.Controller);
    }
  }
}