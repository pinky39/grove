namespace Grove.Core.Effects
{
  using Grove.Core.Targeting;

  public class ReturnToHand : Effect
  {
    public int Discard;
    public Card Card;
    public bool ReturnOwner;    

    protected override void ResolveEffect()
    {
      if (Card != null)
      {
        Card.PutToHand();
      }

      if (HasTargets)
      {
        Target().Card().PutToHand();
      }

      if (ReturnOwner && Target() != Source.OwningCard)
      {
        Source.OwningCard.PutToHand();
      }

      if (Discard > 0)
      {
        Game.Enqueue<Decisions.DiscardCards>(
          controller: Target().Card().Controller,
          init: p => p.Count = Discard);
      }
    }
  }
}