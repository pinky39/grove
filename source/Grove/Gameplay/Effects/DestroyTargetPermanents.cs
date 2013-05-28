namespace Grove.Gameplay.Effects
{
  using System;
  using Artifical;
  using Targeting;

  [Serializable]
  public class DestroyTargetPermanents : Effect
  {
    private readonly bool _canRegenerate;

    private DestroyTargetPermanents() {}

    public DestroyTargetPermanents(bool canRegenerate = true)
    {
      _canRegenerate = canRegenerate;
      Category = EffectCategories.Destruction;
    }

    protected override void ResolveEffect()
    {
      foreach (var target in ValidEffectTargets)
      {
        target.Card().Destroy(_canRegenerate);
      }
    }
  }
}