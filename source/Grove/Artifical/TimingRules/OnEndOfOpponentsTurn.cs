namespace Grove.Artifical.TimingRules
{
  public class OnEndOfOpponentsTurn : TimingRule
  {
    public override bool? ShouldPlay2(TimingRuleParameters p)
    {
      return IsEndOfOpponentsTurn(p.Controller);
    }
  }
}