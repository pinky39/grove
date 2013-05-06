namespace Grove.UserInterface.CardOrder
{
  using Gameplay;

  public class OrderedCard
  {
    public OrderedCard(Card card)
    {
      Card = card;
    }

    public Card Card { get; private set; }
    public virtual int? Order { get; set; }
  }
}