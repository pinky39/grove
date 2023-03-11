namespace Grove
{
  using System.Linq;
  using Events;
  using Infrastructure;
  using Modifiers;

  public class Counters : GameObject, IAcceptsCardModifier
  {
    private readonly TrackableList<Counter> _counters = new TrackableList<Counter>();
    private readonly Strength _strength;
    private Card _owningCard;

    private Counters() { }

    public Counters(Strength strength)
    {
      _strength = strength;
    }

    public int Count
    {
      get { return _counters.Count; }
    }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public void Initialize(Card owningCard, Game game)
    {
      Game = game;
      _owningCard = owningCard;
      _counters.Initialize(ChangeTracker, owningCard);
    }

    public int CountSpecific(CounterType counterType)
    {
      return _counters.Count(x => x.Type == counterType);
    }

    public void Add(Counter counter)
    {
      counter.ModifyStrength(_strength);
      _counters.Add(counter);

      Publish(new CounterAddedEvent(counter, _owningCard));
    }

    public void Remove(Counter counter)
    {
      if (_counters.Remove(counter))
      {
        counter.Remove();
      }

      Publish(new CounterRemovedEvent(counter, _owningCard));
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