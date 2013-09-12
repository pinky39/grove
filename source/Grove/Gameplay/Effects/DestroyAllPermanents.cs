namespace Grove.Gameplay.Effects
{
  using System;
  using Artifical;

  public class DestroyAllPermanents : Effect
  {
    private readonly bool _allowToRegenerate;
    private readonly Func<Effect, Card, bool> _filter;

    private DestroyAllPermanents() {}

    public DestroyAllPermanents(Func<Effect, Card, bool> filter = null, bool allowToRegenerate = true)
    {
      _allowToRegenerate = allowToRegenerate;
      _filter = filter ?? delegate { return true; };
      SetTags(EffectTag.Destroy);
    }

    protected override void ResolveEffect()
    {
      Players.DestroyPermanents(x => _filter(this, x), _allowToRegenerate);
    }
  }
}