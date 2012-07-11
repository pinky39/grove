namespace Grove.Core.Details.Cards.Counters
{
  using System.Linq;
  using Infrastructure;
  using Modifiers;

  [Copyable]
  public class Counters : IModifiable
  {
    private readonly TrackableList<Counter> _counters;
    private readonly Power _power;
    private readonly Toughness _toughness;

    private Counters() {}

    public Counters(Power power, Toughness toughness, ChangeTracker changeTracker, IHashDependancy hashDependancy)
    {
      _power = power;
      _toughness = toughness;
      _counters = new TrackableList<Counter>(changeTracker, hashDependancy);
    }

    public int Count { get { return _counters.Count; } }

    public void Accept(IModifier modifier)
    {
      modifier.Apply(this);
    }

    public int SpecifiCount<T>()
    {
      return _counters.Count(x => x is T);
    }

    [Updates("Count")]
    public virtual void Add(Counter counter)
    {
      counter.ModifyPower(_power);
      counter.ModifyToughness(_toughness);
      _counters.Add(counter);
    }

    [Updates("Count")]
    public virtual void Remove(Counter counter)
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