namespace Grove.Core.Cards.Effects
{
  using Grove.Core.Targeting;

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