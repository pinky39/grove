namespace Grove.Core.Effects
{
  public class DealDamageToOpponentEqualToCardDifference : Effect
  {
    protected override void ResolveEffect()
    {
      var opponent = Players.GetOpponent(Controller);
      var amount = CalculateCardDifference(opponent);

      if (amount > 0)
      {
        var damage = new Damage(
          source: Source.OwningCard,
          amount: amount,
          isCombat: false,
          changeTracker: Game.ChangeTracker
          );

        opponent.DealDamage(damage);
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