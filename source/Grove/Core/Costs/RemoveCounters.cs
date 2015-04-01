namespace Grove.Costs
{
  public class RemoveCounters : Cost
  {
    private readonly int? _count;
    private readonly bool _hasX;
    private readonly CounterType _counterType;

    private RemoveCounters() {}

    public override bool HasX { get { return _hasX; } }

    public RemoveCounters(CounterType counterType, int? count = null, bool hasX = false)
    {
      _count = count;
      _hasX = hasX;
      _counterType = counterType;
    }

    public override CanPayResult CanPayPartial()
    {
      var countersCount = Card.CountersCount(_counterType);
      
      bool canPay = _count == null || countersCount >= _count;
      int? maxX = _hasX ? countersCount : (int?) null;
      return new CanPayResult(canPay, maxX);
    }

    public override void PayPartial(PayCostParameters p)
    {
      var count = _hasX ? p.X : _count;
      Card.RemoveCounters(_counterType, count);
    }
  }
}