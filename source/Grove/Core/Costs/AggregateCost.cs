namespace Grove.Costs
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class AggregateCost : Cost
  {
    private readonly List<Cost> _costs = new List<Cost>();

    private AggregateCost() {}

    public AggregateCost(params Cost[] costs)
    {
      _costs.AddRange(costs);
    }

    public override ManaAmount GetManaCost()
    {      
      var payMana = (PayMana) _costs.SingleOrDefault(x => x is PayMana);
      
      // if there is a child ManaCost use it
      // if not just return the default impl.
      return payMana == null 
        ? base.GetManaCost() 
        : payMana.GetManaCost();
    }

    public override void Initialize(CostType type, Card card, Game game, TargetValidator validator = null)
    {
      base.Initialize(type, card, game, validator);

      foreach (var cost in _costs)
      {
        cost.Initialize(type, card, game, validator);
      }
    }

    public override void PayPartial(PayCostParameters p)
    {
      throw new NotSupportedException("Aggregate cost cannot be child of another one.");
    }

    public override CanPayResult CanPayPartial(bool needsToPayManaCost)
    {
      throw new NotSupportedException("Aggregate cost cannot be child of another one.");
    }

    public override CanPayResult CanPay(bool payManaCost)
    {
      var hasPayMana = HasPayMana();

      if (!hasPayMana && !CanPayAdditionalCost())
      {
        return new CanPayResult(false);
      }
      
      var childResults = _costs
        .Select(cost => cost.CanPayPartial(payManaCost))
        .ToList();

      return new CanPayResult(
        canPay: childResults.All(x => x.CanPay),
        maxX: (childResults.FirstOrDefault(x => x.MaxX.HasValue) ?? childResults.First()).MaxX);
    }

    private bool HasPayMana()
    {
      return _costs.Any(x => x is PayMana);
    }

    public override void Pay(PayCostParameters p)
    {
      // if there is a child pay mana cost
      // additional cost will be payed as
      // part of it.
      // if not we must pay it.
      if (!HasPayMana())
      {        
        PayAdditionalCost();
      }
      
      foreach (var cost in _costs)
      {
        cost.PayPartial(p);
      }
    }
  }
}