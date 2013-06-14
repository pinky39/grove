namespace Grove.Artifical.TimingRules
{
  public class ControllerHasMoreCardsInHand : TimingRule
  {
    private readonly int _minDifference;

    private ControllerHasMoreCardsInHand() {}

    public ControllerHasMoreCardsInHand(int minDifference)
    {
      _minDifference = minDifference;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      var controllerCount = p.Controller.Hand.Count;
      var opponentCount = p.Controller.Opponent.Hand.Count;

      return controllerCount - _minDifference <= opponentCount;
    }
  }
}