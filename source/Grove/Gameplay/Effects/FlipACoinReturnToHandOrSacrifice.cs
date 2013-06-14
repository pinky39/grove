namespace Grove.Gameplay.Effects
{
  public class FlipACoinReturnToHandOrSacrifice : Effect
  {
    protected override void ResolveEffect()
    {
      var result = FlipACoin(Controller);      
      
      if (result)
      {
        Source.OwningCard.PutToHand();
      }
      else
      {
        Source.OwningCard.Sacrifice();
      }
    }
  }
}