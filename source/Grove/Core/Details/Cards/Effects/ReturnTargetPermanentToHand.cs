namespace Grove.Core.Details.Cards.Effects
{
  using Targeting;

  public class ReturnTargetPermanentToHand : Effect
  {
    public int Discard;

    protected override void ResolveEffect()
    {
      Target().Card().ReturnToHand();

      if (Discard > 0)
      {
        Decisions.EnqueueDiscardCards(Target().Card().Controller, Discard);
      }
    }
  }
}