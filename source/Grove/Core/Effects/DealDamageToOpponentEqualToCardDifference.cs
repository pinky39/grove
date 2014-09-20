namespace Grove.Effects
{
  public class DealDamageToOpponentEqualToCardDifference : Effect
  {
    protected override void ResolveEffect()
    {
      var opponent = Players.GetOpponent(Controller);
      var amount = CalculateCardDifference(opponent);

      if (amount > 0)
      {
        Source.OwningCard.DealDamageTo(amount, opponent, isCombat: false);
      }
    }

    private int CalculateCardDifference(Player opponent)
    {
      return Controller.Hand.Count - opponent.Hand.Count;
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return player != Controller ? CalculateCardDifference(player) : 0;
    }
  }
}