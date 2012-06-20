namespace Grove.Core.Effects
{
  public class DestroySource : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.Destroy();
    }
  }
}