namespace Grove.Gameplay.Effects
{
  using Artifical;
  using Targeting;

  public class DestroyTargetPermanents : Effect
  {
    private readonly bool _canRegenerate;

    private DestroyTargetPermanents() {}

    public DestroyTargetPermanents(bool canRegenerate = true)
    {
      _canRegenerate = canRegenerate;

      Tags(EffectTag.Destroy);

      if (!canRegenerate)
        Tags(EffectTag.CannotRegenerate);
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