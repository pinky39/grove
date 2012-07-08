namespace Grove.Core.Effects
{
  public class DestroyTargetPermanent : Effect
  {
    protected override void ResolveEffect()
    {
      Target().Card().Destroy();
    }
  }
}