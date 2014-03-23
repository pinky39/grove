namespace Grove.Effects
{
  using System;
  using System.Linq;
  using Grove.AI;

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
      var permanents = Players.Permanents().Where(c => _filter(this, c)).ToList();

      foreach (var permanent in permanents)
      {
        permanent.Destroy(_allowToRegenerate);
      }            
    }
  }
}