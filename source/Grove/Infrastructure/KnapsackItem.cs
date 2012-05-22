namespace Grove.Infrastructure
{
  public class KnapsackItem<T>
  {
    public T Item { get; private set; }
    public int Weight { get; private set; }
    public int Value { get; private set; }

    public KnapsackItem(T item, int weight, int value)
    {
      Item = item;
      Weight = weight;
      Value = value;
    }
  }
}