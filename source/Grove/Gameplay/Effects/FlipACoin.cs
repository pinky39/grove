namespace Grove.Gameplay.Effects
{
  using Messages;

  public class FlipACoinReturnToHand : Effect
  {
    protected override void ResolveEffect()
    {
      var hasWon = Coin.Flip();

      Publish(new PlayerHasFlippedACoin
        {
          Player = Controller,
          HasWon = hasWon
        });


      if (hasWon)
        return;

      Source.OwningCard.PutToHand();
    }
  }
}