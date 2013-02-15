namespace Grove.Core.Costs
{
  using System;
  using Targeting;

  public class PayLife : Cost
  {
    private readonly Func<Card, int> _getAmount;

    public PayLife(int amount) : this(c => amount) {}

    public PayLife(Func<Card, int> getAmount)
    {
      _getAmount = getAmount;
    }

    public override bool CanPay(ref int? maxX)
    {
      return _getAmount(Card) <= Card.Controller.Life;
    }

    protected override void Pay(ITarget target, int? x)
    {
      Card.Controller.Life -= _getAmount(Card);
    }
  }
}