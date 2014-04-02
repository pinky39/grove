namespace Grove.Events
{
  public class CounterRemoved
  {
    public readonly Counter Counter;
    public readonly Card OwningCard;

    public CounterRemoved(Counter counter, Card owningCard)
    {
      Counter = counter;
      OwningCard = owningCard;
    }
  }
}