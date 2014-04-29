namespace Grove.Events
{
  public class PermanentUntappedEvent
  {
    public readonly Card Card;

    public PermanentUntappedEvent(Card card)
    {
      Card = card;
    }
  }
}