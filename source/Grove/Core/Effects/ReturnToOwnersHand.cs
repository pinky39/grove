namespace Grove.Core.Effects
{
  public class ReturnToOwnersHand : Effect
  {
    protected override void ResolveEffect()
    {
      Controller.ReturnToHand(Source.OwningCard);
    }
  }
}