namespace Grove.Core.Effects
{
  public class ShuffleIntoLibrary : Effect
  {
    public override void Resolve()
    {
      Controller.ShuffleIntoLibrary(Source.OwningCard);
    }    
  }
}