namespace Grove.Events
{
  public class CounterAddedEvent
  {
    public readonly Counter Counter;
    public readonly Card OwningCard;

    public CounterAddedEvent(Counter counter, Card owningCard)
    {
      Counter = counter;
      OwningCard = owningCard;
    }
  }
}