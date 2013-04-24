namespace Grove.Gameplay.Effects
{
  public class UntapOwner : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.Untap();
    }
  }
}