namespace Grove.Core.Effects
{
  using System;

  public class DestroyAllPermanents : Effect
  {
    public Func<DestroyAllPermanents, Card, bool> Filter;

    protected override void ResolveEffect()
    {
      Core.Players.DestroyPermanents(x => Filter(this, x));
    }
  }
}