namespace Grove.Effects
{
  using System;
  using System.Linq;

  public class RemoveAllCountersFromPermanents : Effect
  {
    private readonly CounterType _counterType;
    private readonly Func<Card, bool> _filter;

    private RemoveAllCountersFromPermanents() {}

    public RemoveAllCountersFromPermanents(Func<Card, bool> filter, CounterType counterType)
    {
      _filter = filter;
      _counterType = counterType;
    }

    protected override void ResolveEffect()
    {
      foreach (var permanent in Players.Permanents().Where(_filter))
      {
        permanent.RemoveCounters(_counterType);
      }
    }
  }
}