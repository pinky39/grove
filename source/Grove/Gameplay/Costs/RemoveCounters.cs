namespace Grove.Gameplay.Costs
{
  using Counters;
  using Targeting;

  public class RemoveCounters : Cost
  {
    private readonly int? _count;
    private readonly CounterType _counterType;

    private RemoveCounters() {}

    public RemoveCounters(CounterType counterType, int? count = null)
    {
      _count = count;
      _counterType = counterType;
    }

    protected override void CanPay(CanPayResult result)
    {            
      result.CanPay = _count == null || Card.CountersCount(_counterType) >= _count;
    }

    protected override void PayCost(Targets targets, int? x, int repeat)
    {            
      Card.RemoveCounters(_counterType, _count);
    }
  }
}