namespace Grove.Core.Effects
{
  public class ReturnToOwnersHand : Effect
  {
    public override void Resolve()
    {
      Controller.ReturnToHand(Source.OwningCard);
    }
  }
}