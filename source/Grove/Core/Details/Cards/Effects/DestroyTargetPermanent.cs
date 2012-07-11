namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class DestroyTargetPermanent : Effect
  {
    public bool AllowRegenerate = true;

    protected override void ResolveEffect()
    {
      Target().Card().Destroy(AllowRegenerate);
    }
  }
}