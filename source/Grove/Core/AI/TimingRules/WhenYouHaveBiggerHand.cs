namespace Grove.AI.TimingRules
{
  public class WhenYouHaveBiggerHand : TimingRule
  {
    private readonly int _minDifference;

    private WhenYouHaveBiggerHand() {}

    public WhenYouHaveBiggerHand(int minDifference)
    {
      _minDifference = minDifference;
    }

    public override bool ShouldPlayAfterTargets(TimingRuleParameters p)
    {
      var controllerCount = p.Controller.Hand.Count;
      var opponentCount = p.Controller.Opponent.Hand.Count;

      return controllerCount - _minDifference <= opponentCount;
    }
  }
}