namespace Grove.Ai.TimingRules
{
  public class OpponentHasAtLeastCardsInHand : TimingRule
  {
    private readonly int _count;

    private OpponentHasAtLeastCardsInHand() {}

    public OpponentHasAtLeastCardsInHand(int count)
    {
      _count = count;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return p.Controller.Opponent.Hand.Count >= _count;
    }
  }
}