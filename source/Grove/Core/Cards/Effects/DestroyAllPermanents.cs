namespace Grove.Core.Cards.Effects
{
  using System;

  public class DestroyAllPermanents : Effect
  {
    public Func<Card, bool> Filter;

    protected override void ResolveEffect()
    {
      Players.DestroyPermanents(Filter);
    }
  }
}