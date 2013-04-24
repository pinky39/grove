namespace Grove.Gameplay.Messages
{
  using Card;

  public class ControllerChanged
  {
    public Card Card { get; private set; }

    public ControllerChanged(Card card)
    {
      Card = card;      
    }
  }
}