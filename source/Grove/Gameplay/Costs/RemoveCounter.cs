namespace Grove.Gameplay.Costs
{
  using System;
  using Counters;
  using Targeting;

  [Serializable]
  public class RemoveCounter : Cost
  {
    private readonly CounterType _counterType;

    private RemoveCounter() {}

    public RemoveCounter(CounterType counterType)
    {
      _counterType = counterType;
    }

    protected override void CanPay(CanPayResult result)
    {
      result.CanPay = Card.CountersCount(_counterType) > 0;
    }

    protected override void Pay(ITarget target, int? x, int repeat)
    {
      Card.RemoveCounters(_counterType, 1);
    }
  }
}