namespace Grove.Gameplay.Effects
{
  using System;
  using System.Linq;
  using Counters;

  public class RemoveAllCounters : Effect
  {
    private readonly CounterType _counterType;
    private readonly Func<Card, bool> _filter;

    private RemoveAllCounters() {}

    public RemoveAllCounters(Func<Card, bool> filter, CounterType counterType)
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