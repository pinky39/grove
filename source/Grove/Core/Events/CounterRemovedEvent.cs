namespace Grove.Events
{
  public class CounterRemovedEvent
  {
    public readonly Counter Counter;
    public readonly Card OwningCard;

    public CounterRemovedEvent(Counter counter, Card owningCard)
    {
      Counter = counter;
      OwningCard = owningCard;
    }
  }
}