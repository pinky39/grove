namespace Grove.Effects
{
  public class FlipCoinDealDamageToItself : Effect
  {
    private readonly int _amount;

    private FlipCoinDealDamageToItself() {}

    public FlipCoinDealDamageToItself(int amount)
    {
      _amount = amount;
    }

    protected override void ResolveEffect()
    {
      var hasWon = FlipACoin(Controller);

      if (hasWon)
        return;

      Source.OwningCard.DealDamageTo(
          _amount,
          Source.OwningCard,
          isCombat: false);
    }
  }
}
