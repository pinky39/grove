namespace Grove.Core.Messages
{
  public class ControllerChanged
  {
    public Card Card { get; private set; }

    public ControllerChanged(Card card)
    {
      Card = card;      
    }
  }
}