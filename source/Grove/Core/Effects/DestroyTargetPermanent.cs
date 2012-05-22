namespace Grove.Core.Effects
{
  public class DestroyTargetPermanent : Effect
  {
    public override void Resolve()
    {
      Target.Card().Destroy();
    }
  }
}