namespace Grove.Effects
{
  public class ShuffleOwningCardIntoLibrary : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.ShuffleIntoLibrary();
    }
  }
}