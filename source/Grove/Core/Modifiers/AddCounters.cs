namespace Grove.Modifiers
{
  using System;
  using System.Collections.Generic;

  public class AddCounters : Modifier, ICardModifier
  {
    public delegate Value GetCount(Context ctx);
    
    private readonly List<Counter> _addedCounters = new List<Counter>();
    private readonly GetCount _getCount;
    private readonly Func<Counter> _counterFactory;

    private Counters _counters;

    private AddCounters() {}    

    public AddCounters(Func<Counter> counter, Value count)
      : this(counter, (_) => count ?? 1)
    {
    }

    public AddCounters(Func<Counter> counter, GetCount count)
    {
      _counterFactory = counter;
      _getCount = count;
    }

    public override void Apply(Counters counters)
    {
      _counters = counters;
      var count = _getCount(Ctx).GetValue(X);

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