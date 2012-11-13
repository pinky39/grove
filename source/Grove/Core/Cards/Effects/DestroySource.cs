namespace Grove.Core.Cards.Effects
{
  public class DestroySource : Effect
  {
    protected override void ResolveEffect()
    {      
      Source.OwningCard.Destroy();
    }
  }
}