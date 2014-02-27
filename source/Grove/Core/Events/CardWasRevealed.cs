namespace Grove.Events
{
  public class CardWasRevealed
  {
    public Card Card { get; set; }

    public override string ToString()
    {
      return string.Format("{0} revealed {1}.", Card.Controller, Card);
    }
  }
}