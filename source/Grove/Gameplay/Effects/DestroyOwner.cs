namespace Grove.Gameplay.Effects
{
  public class DestroyOwner : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.Destroy();
    }
  }
}