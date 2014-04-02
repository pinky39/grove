namespace Grove.Events
{
  public class CounterAdded
  {
    public readonly Counter Counter;
    public readonly Card OwningCard;

    public CounterAdded(Counter counter, Card owningCard)
    {
      Counter = counter;
      OwningCard = owningCard;
    }
  }
}