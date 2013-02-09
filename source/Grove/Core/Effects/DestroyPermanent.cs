namespace Grove.Core.Effects
{
  using System;
  using Ai;

  public class DestroyPermanent : Effect
  {
    private readonly Func<Effect, Card> _selector;

    private DestroyPermanent() {}

    public DestroyPermanent(Func<Effect, Card> selector)
    {
      _selector = selector;
      Category = EffectCategories.Destruction;
    }


    protected override void ResolveEffect()
    {
      _selector(this).Destroy();
    }
  }
}