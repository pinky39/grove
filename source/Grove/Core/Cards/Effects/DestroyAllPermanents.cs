namespace Grove.Core.Cards.Effects
{
  using System;

  public class DestroyAllPermanents : Effect
  {
    public Func<DestroyAllPermanents, Card, bool> Filter;

    protected override void ResolveEffect()
    {
      Players.DestroyPermanents(x => Filter(this, x));
    }
  }
}