namespace Grove.Gameplay.Effects
{
  using System;
  using Messages;
  using Misc;

  [Serializable]
  public class FlipACoinReturnToHand : Effect
  {
    private readonly Coin _coin = new Coin();

    protected override void ResolveEffect()
    {
      var hasWon = _coin.Flip();

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