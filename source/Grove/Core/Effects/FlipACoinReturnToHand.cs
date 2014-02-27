namespace Grove.Effects
{
  public class FlipACoinReturnToHand : Effect
  {
    protected override void ResolveEffect()
    {                  
      var hasWon = FlipACoin(Controller);

      if (hasWon)
        return;

      Source.OwningCard.PutToHandFrom(Zone.Battlefield);
    }
  }
}