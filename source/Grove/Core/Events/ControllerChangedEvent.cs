namespace Grove.Events
{
  public class ControllerChangedEvent
  {
    public readonly Card Card;

    public ControllerChangedEvent(Card card)
    {
      Card = card;
    }
  }
}