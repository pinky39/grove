namespace Grove.Effects
{
  using System;
  using System.Linq;
  using AI;

  public class DestroyAllPermanents : Effect
  {
    private readonly bool _allowToRegenerate;
    private readonly DynParam<int> _countOnInit;
    private readonly Func<Effect, int, Card, bool> _filter;

    private DestroyAllPermanents() {}

    public DestroyAllPermanents(bool allowToRegenerate = true)
      : this(delegate { return true; }, allowToRegenerate) {}

    public DestroyAllPermanents(Func<Effect, Card, bool> filter, bool allowToRegenerate = true)
      : this((e, count, card) => filter(e, card), 0, allowToRegenerate) {}

    public DestroyAllPermanents(
      Func<Effect, int, Card, bool> filter,
      DynParam<int> countOnInit,
      bool allowToRegenerate = true)
    {
      _countOnInit = countOnInit;
      _allowToRegenerate = allowToRegenerate;
      _filter = filter;

      SetTags(EffectTag.Destroy);
      
      if (!allowToRegenerate)
        SetTags(EffectTag.CannotRegenerate);
      
      RegisterDynamicParameters(countOnInit);
    }

    protected override void ResolveEffect()
    {
      var permanents = Players.Permanents()
        .Where(c => _filter(this, _countOnInit.Value, c)).ToList();

      foreach (var permanent in permanents)
      {
        permanent.Destroy(_allowToRegenerate);
      }
    }
  }
}