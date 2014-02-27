namespace Grove.UserInterface.CardOrder
{
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