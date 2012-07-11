namespace Grove.Core.Details.Cards.Effects
{
  public class DestroySource : Effect
  {
    protected override void ResolveEffect()
    {
      Source.OwningCard.Destroy();
    }
  }
}