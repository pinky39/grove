namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class DestroyTargetPermanents : Effect
  {
    public bool AllowRegenerate = true;

    protected override void ResolveEffect()
    {
      foreach (var target in ValidTargets)
      {
        target.Card().Destroy(AllowRegenerate);  
      }            
    }    
  }
}