namespace Grove.Gameplay
{
  using System.Linq;
  using Grove.Infrastructure;
  using Grove.Gameplay.Modifiers;

  [Copyable]
  public class Counters : IAcceptsCardModifier
  {
    private readonly TrackableList<Counter> _counters = new TrackableList<Counter>();
    private readonly Strenght _strenght;

    private Counters() {}

    public Counters(Strenght strenght)
    {
      _strenght = strenght;
    }

    public int Count { get { return _counters.Count; } }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _counters.Initialize(changeTracker, hashDependancy);
    }

    public int CountSpecific(CounterType counterType)
    {
      return _counters.Count(x => x.Type == counterType);
    }

    public void Add(Counter counter)
    {
      counter.ModifyStrenght(_strenght);
      _counters.Add(counter);
    }

    public void Remove(Counter counter)
    {
      if (_counters.Remove(counter))
      {
        counter.Remove();
      }
    }

    public void Remove(CounterType counterType, int? count = null)
    {
      var counters = _counters.Where(x => x.Type == counterType);

      if (count != null)
      {
        counters = counters.Take(count.Value);
      }

      foreach (var counter in counters.ToArray())
      {
        Remove(counter);
      }
    }
  }
}