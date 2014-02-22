namespace Grove.Gameplay
{
  public class SimpleCounter : Counter
  {
    private readonly CounterType _type;

    private SimpleCounter() {}

    public SimpleCounter(CounterType type)
    {
      _type = type;
    }

    public override CounterType Type { get { return _type; } }

    public override void Remove() {}
  }
}