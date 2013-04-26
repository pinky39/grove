namespace Grove.Gameplay.Card.Counters
{
  using System.Linq;
  using Characteristics;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class Counters : IModifiable
  {
    private readonly TrackableList<Counter> _counters = new TrackableList<Counter>();
    private readonly Power _power;
    private readonly Toughness _toughness;

    private Counters() {}

    public Counters(Power power, Toughness toughness)
    {
      _power = power;
      _toughness = toughness;
    }

    public int Count { get { return _counters.Count; } }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _counters.Initialize(changeTracker, hashDependancy);
    }

    public int CountSpecific<T>()
    {
      return _counters.Count(x => x is T);
    }

    public void Add(Counter counter)
    {
      counter.ModifyPower(_power);
      counter.ModifyToughness(_toughness);
      _counters.Add(counter);
    }

    public void Remove(Counter counter)
    {
      if (_counters.Remove(counter))
      {
        counter.Remove();
      }
    }

    public void RemoveAny<T>()
    {
      var counter = _counters.FirstOrDefault(x => x is T);

      if (counter == null)
        return;

      Remove(counter);
    }
  }
}