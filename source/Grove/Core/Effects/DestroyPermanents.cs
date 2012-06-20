namespace Grove.Core.Effects
{
  using System;

  public class DestroyPermanents : Effect
  {
    public Func<Card, bool> Filter = null;

    protected override void ResolveEffect()
    {
      Players.DestroyPermanents(Filter);
    }
  }
}