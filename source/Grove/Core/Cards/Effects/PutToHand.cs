namespace Grove.Core.Cards.Effects
{
  using Targeting;

  public class PutToHand : Effect
  {
    public int Discard;
    public Card Card;
    public bool AlsoReturnOwner;    

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

      if (AlsoReturnOwner && Target() != Source.OwningCard)
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