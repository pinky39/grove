namespace Grove.Costs
{
  public class PayMana : Cost
  {
    private readonly ManaAmount _amount;
    private readonly bool _hasX;   
    private readonly bool _supportsRepetitions;

    private PayMana() {}

    public PayMana(
      ManaAmount amount,
      bool hasX = false,
      bool supportsRepetitions = false)
    {
      _amount = amount;
      
      _hasX = hasX;
      _supportsRepetitions = supportsRepetitions;
    }

    public override bool HasX { get { return _hasX; } }

    public override ManaAmount GetManaCost()
    {
      var change = Game.GetCostChange(Type, Card);

      if (change == 0)
        return _amount;

      return change > 0
        ? _amount.Add(change.Colorless())
        : _amount.Remove((-change).Colorless());
    }

    public override CanPayResult CanPay(bool payManaCost)
    {
      var actualCost = payManaCost ? GetManaCost() : Mana.Zero;

      int? maxX = null;
      var maxRepetitions = 1;
      
      var canPay = Controller.HasMana(
        actualCost,
        GetManaUsage(),
        canUseConvoke: Card.Has().Convoke,
        canUseDelve: Card.Has().Delve);

      if (canPay)
      {
        if (_hasX)
        {
          var availableMana = Controller.GetAvailableManaCount(GetManaUsage(), canUseConvoke: Card.Has().Convoke,
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

            if (!Controller.HasMana(amount, GetManaUsage()))
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

    public override CanPayResult CanPayPartial(bool needsToPayManaCost)
    {
      return CanPay(needsToPayManaCost);
    }

    public override void PayPartial(PayCostParameters p)
    {
      Pay(p);
    }

    public override void Pay(PayCostParameters p)
    {
      var amount = p.PayManaCost ? GetManaCost() : Mana.Zero;

      if (_hasX)
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

      Controller.Consume(amount, GetManaUsage(), canUseConvoke: Card.Has().Convoke, canUseDelve: Card.Has().Delve);
    }
  }
}