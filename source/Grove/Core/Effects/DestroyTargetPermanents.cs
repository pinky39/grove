namespace Grove.Effects
{
  using Grove.AI;

  public class DestroyTargetPermanents : Effect
  {
    private readonly bool _canRegenerate;

    private DestroyTargetPermanents() {}

    public DestroyTargetPermanents(bool canRegenerate = true)
    {
      _canRegenerate = canRegenerate;

      SetTags(EffectTag.Destroy);

      if (!canRegenerate)
        SetTags(EffectTag.CannotRegenerate);
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