namespace Grove.Core.Details.Cards.Effects
{
  public class ShuffleIntoLibrary : Effect
  {
    protected override void ResolveEffect()
    {
      Controller.ShuffleIntoLibrary(Source.OwningCard);
    }
  }
}