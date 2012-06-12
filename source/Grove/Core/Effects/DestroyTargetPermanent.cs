namespace Grove.Core.Effects
{
  using System;

  public class DestroyTargetPermanent : Effect
  {
    public override void Resolve()
    {
      Target.Card().Destroy();
    }
  }
}