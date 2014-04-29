namespace Grove.Events
{
  public class CardWasRevealedEvent
  {
    public readonly Card Card;

    public CardWasRevealedEvent(Card card)
    {
      Card = card;
    }

    public override string ToString()
    {
      return string.Format("{0} revealed {1}.", Card.Controller, Card);
    }
  }
}