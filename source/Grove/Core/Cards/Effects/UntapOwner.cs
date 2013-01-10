namespace Grove.Core.Cards.Effects
{
  public class UntapOwner : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.Untap();
    }
  }
}