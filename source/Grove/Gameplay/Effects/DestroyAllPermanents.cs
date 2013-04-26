namespace Grove.Gameplay.Effects
{
  using System;
  using Ai;
  using Card;

  public class DestroyAllPermanents : Effect
  {
    private readonly Func<Effect, Card, bool> _filter;

    private DestroyAllPermanents() {}

    public DestroyAllPermanents(Func<Effect, Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
      Category = EffectCategories.Destruction;
    }

    protected override void ResolveEffect()
    {
      Players.DestroyPermanents(x => _filter(this, x));
    }
  }
}