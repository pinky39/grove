namespace Grove.Core.Effects
{
  public class DestroySource : Effect
  {
    public override void Resolve()
    {
      Source.OwningCard.Destroy();
    }
  }
}