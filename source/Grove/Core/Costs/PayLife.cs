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

    protected override void CanPay(CanPayResult result)
    {
      result.CanPay(() => _getAmount(Card) <= Card.Controller.Life);

      if (_supportsRepetitions)
      {
        result.MaxRepetitions(() => Card.Controller.Life - 1);
      }
    }

    protected override void PayCost(Targets targets, int? x, int repeat)
    {      
      Card.Controller.Life -= _getAmount(Card) * repeat;
    }
  }
}