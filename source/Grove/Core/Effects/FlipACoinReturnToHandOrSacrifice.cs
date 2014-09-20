namespace Grove.Effects
{
  public class FlipACoinReturnToHandOrSacrifice : Effect
  {
    protected override void ResolveEffect()
    {
      var result = FlipACoin(Controller);      
      
      if (result)
      {
        Source.OwningCard.PutToHandFrom(Zone.Battlefield);
      }
      else
      {
        Source.OwningCard.Sacrifice();
      }
    }
  }
}