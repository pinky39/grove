namespace Grove.Gameplay.Effects
{
  using Messages;

  public class FlipACoinReturnToHand : Effect
  {
    protected override void ResolveEffect()
    {
      var hasWon = FlipACoin(Controller);    

      if (hasWon)
        return;

      Source.OwningCard.PutToHand();
    }
  }
}