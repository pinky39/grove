namespace Grove.Costs
{
  public class PayMana : Cost
  {
    private readonly IManaAmount _amount;
    private readonly bool _hasX;
    private readonly ManaUsage _manaUsage;
    private readonly bool _supportsRepetitions;

    private PayMana() {}

    public PayMana(
      IManaAmount amount,
      ManaUsage manaUsage,
      bool hasX = false,
      bool supportsRepetitions = false)
    {
      _amount = amount;
      _manaUsage = manaUsage;
      _hasX = hasX;
      _supportsRepetitions = supportsRepetitions;
    }

    public override bool HasX { get { return _hasX; } }

    public override IManaAmount GetManaCost()
    {
      var change = Game.GetCostChange(Type, Card);

      if (change == 0)
        return _amount;

      return change > 0
        ? _amount.Add(change.Colorless())
        : _amount.Remove((-change).Colorless());
    }

    public override CanPayResult CanPay()
    {
      var actualCost = GetManaCost();

      int? maxX = null;
      var maxRepetitions = 1;

      var canPay = Controller.HasMana(
        actualCost,
        _manaUsage,
        canUseConvoke: Card.Has().Convoke,
        canUseDelve: Card.Has().Delve);

      if (canPay)
      {
        if (_hasX)
        {
          var availableMana = Controller.GetAvailableConvertedMana(_manaUsage, canUseConvoke: Card.Has().Convoke,
            canUseDelve: Card.Has().Delve);
          maxX = availableMana - actualCost.Converted;
        }

        if (_supportsRepetitions)
        {
          var count = 1;
          var amount = actualCost;

          while (true)
          {
            amount = amount.Add(actualCost);

            if (!Controller.HasMana(amount, _manaUsage))
            {
              break;
            }
            count++;
          }

          maxRepetitions = count;
        }
      }

      return new CanPayResult(canPay, maxX, maxRepetitions);
    }

    public override CanPayResult CanPayPartial()
    {
      return CanPay();
    }

    public override void PayPartial(PayCostParameters p)
    {
      Pay(p);
    }

    public override void Pay(PayCostParameters p)
    {
      var amount = GetManaCost();

      if (p.X.HasValue)
      {
        amount = amount.Add(p.X.Value.Colorless());
      }

      if (_supportsRepetitions)
      {
        for (var i = 1; i < p.Repeat; i++)
        {
          amount = amount.Add(_amount);
        }
      }

      Controller.Consume(amount, _manaUsage, canUseConvoke: Card.Has().Convoke, canUseDelve: Card.Has().Delve);
    }
  }
}