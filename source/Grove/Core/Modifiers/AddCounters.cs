namespace Grove.Core.Modifiers
{
  using System;
  using System.Collections.Generic;
  using Counters;

  public class AddCounters : Modifier
  {
    private readonly List<Counter> _addedCounters = new List<Counter>();
    private readonly Value _count;
    private readonly Func<Counter> _counterFactory;

    private Counters _counters;

    public AddCounters(Func<Counter> counter, Value count)
    {
      _counterFactory = counter;
      _count = count ?? 1;
    }

    public override void Apply(Counters counters)
    {
      _counters = counters;

      for (var i = 0; i < _count.GetValue(X); i++)
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