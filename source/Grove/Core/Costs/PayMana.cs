namespace Grove.Costs
{
  using System;

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

    public override bool HasX
    {
      get { return _hasX; }
    }

    public override IManaAmount GetManaCost()
    {
      return _amount;
    }

    protected override void CanPay(CanPayResult result)
    {
      // mana checking is an expensive operation
      // so it should only be done when nessesary
      // the following lazy evaluation allows ai
      // to only check mana costs when all the cheaper
      // timing tests are successful

      var evaluator = new PayManaEvaluator(this);

      result.CanPay(evaluator.CanPay);
      result.MaxX(evaluator.MaxX);
      result.MaxRepetitions(evaluator.MaxRepetitions);
    }

    public override void Pay(PayCostParameters p)
    {
      var amount = Game.GetActualCost(_amount, _manaUsage, Card);

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

      Card.Controller.Consume(amount, _manaUsage, canUseConvoke: Card.Has().Convoke, canUseDelve: Card.Has().Delve);
    }

    private class PayManaEvaluator
    {
      private readonly PayMana _payMana;
      private bool _canPay;
      private bool _isEvaluated;
      private int _maxRepetitions = 1;
      private int? _maxX;

      public PayManaEvaluator(PayMana payMana)
      {
        _payMana = payMana;
      }

      public Func<bool> CanPay
      {
        get
        {
          return () =>
            {
              Evaluate();
              return _canPay;
            };
        }
      }

      public Func<int?> MaxX
      {
        get
        {
          return () =>
            {
              if (!_payMana._hasX)
                return null;

              Evaluate();
              return _maxX;
            };
        }
      }

      public Func<int> MaxRepetitions
      {
        get
        {
          return () =>
            {
              if (!_payMana._supportsRepetitions)
                return 1;

              Evaluate();
              return _maxRepetitions;
            };
        }
      }

      private void Evaluate()
      {
        if (_isEvaluated)
          return;

        var manaUsage = _payMana._manaUsage;
        var controller = _payMana.Card.Controller;

        var actualCost = _payMana.Game.GetActualCost(_payMana._amount, manaUsage, _payMana.Card);
        _canPay = controller.HasMana(actualCost, manaUsage, canUseConvoke: _payMana.Card.Has().Convoke, canUseDelve: _payMana.Card.Has().Delve);

        if (_canPay)
        {
          if (_payMana._hasX)
          {
            _maxX = controller.GetAvailableConvertedMana(manaUsage, canUseConvoke: _payMana.Card.Has().Convoke, canUseDelve: _payMana.Card.Has().Delve) - actualCost.Converted;
          }

          if (_payMana._supportsRepetitions)
          {
            var count = 1;
            var amount = actualCost;

            while (true)
            {
              amount = amount.Add(actualCost);

              if (!controller.HasMana(amount, manaUsage))
              {
                break;
              }
              count++;
            }

            _maxRepetitions = count;
          }
        }

        _isEvaluated = true;
      }
    }
  }
}