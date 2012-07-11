namespace Grove.Core.Details.Cards.Modifiers
{
  using System.Collections.Generic;
  using Counters;

  public class AddCounters : Modifier
  {
    private readonly List<Counter> _addedCounters = new List<Counter>();

    public Value Count = 1;
    private Counters _counters;
    public ICounterFactory Counter { get; set; }

    public override void Apply(Counters counters)
    {
      _counters = counters;

      var count = Count.GetValue(X);

      for (var i = 0; i < count; i++)
      {
        var counter = Counter.Create();
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