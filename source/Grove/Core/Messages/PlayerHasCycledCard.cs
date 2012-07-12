namespace Grove.Core.Messages
{
  public class PlayerHasCycledCard
  {
    public PlayerHasCycledCard(Card card)
    {
      Card = card;
    }

    public Card Card { get; private set; }

    public override string ToString()
    {
      return string.Format("{0} cycled {1}.", Card.Controller, Card);
    }
  }
}