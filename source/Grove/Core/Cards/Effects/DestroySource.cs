namespace Grove.Core.Details.Cards.Effects
{
  using System;

  public class DestroySource : Effect
  {
    protected override void ResolveEffect()
    {      
      Source.OwningCard.Destroy();
    }
  }
}