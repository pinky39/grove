namespace Grove.Modifiers
{
  using System;
  using System.Collections.Generic;

  public class AddCounters : Modifier, ICardModifier
  {
    private readonly List<Counter> _addedCounters = new List<Counter>();
    private readonly Func<Game, Value> _getCount;
    private readonly Func<Counter> _counterFactory;

    private Counters _counters;

    private AddCounters() {}    

    public AddCounters(Func<Counter> counter, Value count)
      : this(counter, (g) => count ?? 1)
    {
    }

    public AddCounters(Func<Counter> counter, Func<Game, Value> getCount)
    {
      _counterFactory = counter;
      _getCount = getCount;
    }

    public override void Apply(Counters counters)
    {
      _counters = counters;
      var count = _getCount(Game).GetValue(X);

      for (var i = 0; i < count; i++)
      {
        var counter = _counterFactory().Initialize(Game);
        counters.Add(counter);

        _addedCounters.Add(counter);
      }
    }

    protected override void Unapply()
    {
      foreach (var counter in _addedCounters)
      {
        _counters.Remove(counter);
      }
    }
  }
}