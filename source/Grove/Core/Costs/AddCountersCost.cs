namespace Grove.Costs
{
  using Modifiers;

  public class AddCountersCost : Cost
  {
    private readonly int? _count;
    private readonly bool _hasX;
    private readonly CounterType _counterType;

    private AddCountersCost() {}

    public AddCountersCost(CounterType counterType, int? count = null, bool hasX = false)
    {
      _count = count;
      _hasX = hasX;
      _counterType = counterType;
    }

    public override CanPayResult CanPayPartial(bool needsToPayManaCost)
    {
      return true;
    }

    public override void PayPartial(PayCostParameters p)
    {
      var count = _hasX ? p.X : _count;

      var mp = new ModifierParameters {SourceCard = Card};

      Card.AddModifier(new AddCounters(
        () => new SimpleCounter(_counterType), count), mp);
    }
  }
}