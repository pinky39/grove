namespace Grove.Events
{
  public class TypeChangedEvent
  {
    public readonly Card Card;
    public readonly CardType NewValue;
    public readonly CardType OldValue;

    public TypeChangedEvent(Card card, CardType oldValue, CardType newValue)
    {
      Card = card;
      OldValue = oldValue;
      NewValue = newValue;
    }
  }
}