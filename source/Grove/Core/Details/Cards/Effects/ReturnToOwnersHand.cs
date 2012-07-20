namespace Grove.Core.Details.Cards.Effects
{
  public class ReturnToOwnersHand : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.ReturnToHand();
    }
  }
}