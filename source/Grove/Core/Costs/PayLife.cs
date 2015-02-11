namespace Grove.Costs
{
  using System;

  public class PayLife : Cost
  {
    private readonly Func<Card, int> _getAmount;
    private readonly bool _supportsRepetitions;

    private PayLife() {}

    public PayLife(int amount, bool supportsRepetitions = false) : this(c => amount, supportsRepetitions) {}

    public PayLife(Func<Card, int> getAmount, bool supportsRepetitions = false)
    {
      _getAmount = getAmount;
      _supportsRepetitions = supportsRepetitions;
    }

    public override CanPayResult CanPayPartial()
    {
      return new CanPayResult(
        canPay: _getAmount(Card) <= Card.Controller.Life,
        maxRepetitions: _supportsRepetitions ? Card.Controller.Life - 1 : 1);            
    }

    public override void PayPartial(PayCostParameters p)
    {      
      Card.Controller.Life -= _getAmount(Card) * p.Repeat;
    }
  }
}