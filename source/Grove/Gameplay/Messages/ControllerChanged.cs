namespace Grove.Gameplay.Messages
{
  using Card;

  public class ControllerChanged
  {
    public ControllerChanged(Card card)
    {
      Card = card;
    }

    public Card Card { get; private set; }
  }
}