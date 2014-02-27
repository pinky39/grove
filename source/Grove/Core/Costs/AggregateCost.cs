namespace Grove.Costs
{
  using System.Collections.Generic;

  public class AggregateCost : Cost
  {
    private readonly List<Cost> _costs = new List<Cost>();

    private AggregateCost() {}

    public AggregateCost(params Cost[] costs)
    {
      _costs.AddRange(costs);
    }

    public override IManaAmount GetManaCost()
    {
      var manaAmount = Mana.Zero;

      foreach (var cost in _costs)
      {
        manaAmount = manaAmount.Add(cost.GetManaCost());
      }

      return manaAmount;
    }

    public override void Initialize(Card card, Game game, TargetValidator validator = null)
    {
      base.Initialize(card, game, validator);

      foreach (var cost in _costs)
      {
        cost.Initialize(card, game, validator);
      }
    }

    protected override void CanPay(CanPayResult result)
    {
      var childResults = new List<CanPayResult>();

      foreach (var cost in _costs)
      {
        childResults.Add(cost.CanPay());
      }

      result.CanPay(() =>
        {
          foreach (var childResult in childResults)
          {
            if (!childResult.CanPay().Value)
              return false;
          }

          return true;
        });

      result.MaxX(() =>
        {
          int? maxX = null;

          foreach (var childResult in childResults)
          {
            maxX = childResult.MaxX().Value;

            if (maxX.HasValue)
            {
              break;
            }
          }

          return maxX;
        });

      result.MaxRepetitions(() =>
        {
          var maxRepetitions = 1;

          foreach (var childResult in childResults)
          {
            maxRepetitions = childResult.MaxRepetitions().Value;
          }

          return maxRepetitions;
        });
    }

    public override void Pay(Targets targets, int? x, int repeat = 1)
    {
      foreach (var cost in _costs)
      {
        cost.Pay(targets, x, repeat);
      }
    }
  }
}