namespace Grove.Gameplay.Effects
{
  public class ShuffleIntoLibrary : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.ShuffleIntoLibrary();
    }
  }
}