namespace Grove.Core.Ai.TimingRules
{
  public class OpponentHasAtLeastCardsInHand : TimingRule
  {
    public int Count;
    
    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return p.Controller.Opponent.Hand.Count >= Count;
    }
  }
}