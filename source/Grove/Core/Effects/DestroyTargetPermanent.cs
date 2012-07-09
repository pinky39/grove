namespace Grove.Core.Effects
{
  public class DestroyTargetPermanent : Effect
  {
    public bool AllowRegenerate = true;
    
    protected override void ResolveEffect()
    {
      Target().Card().Destroy(AllowRegenerate);
    }
  }
}