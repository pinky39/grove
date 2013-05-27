namespace Grove.Gameplay.Costs
{
  using System;
  using Targeting;

  public class PayLife : Cost
  {
    private readonly Func<Card, int> _getAmount;

    private PayLife() {}

    public PayLife(int amount) : this(c => amount) {}

    public PayLife(Func<Card, int> getAmount)
    {
      _getAmount = getAmount;
    }

    protected override void CanPay(CanPayResult result)
    {
      result.CanPay = _getAmount(Card) <= Card.Controller.Life;
    }


    protected override void Pay(ITarget target, int? x, int repeat)
    {
      Card.Controller.Life -= _getAmount(Card);
    }
  }
}