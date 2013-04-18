namespace Grove.Core.Costs
{
  using System.Collections.Generic;
  using Mana;
  using Targeting;

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
      IManaAmount manaAmount = Mana.Zero;

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
      foreach (var cost in _costs)
      {
        var childResult = cost.CanPay();
        
        result.CanPay = childResult.CanPay;
        result.MaxX = result.MaxX ?? childResult.MaxX;
        result.MaxRepetitions = childResult.MaxRepetitions;

        if (!result.CanPay)
          return;
      }            
    }    

    public override void Pay(Targets targets, int? x)
    {
      foreach (var cost in _costs)
      {
        cost.Pay(targets, x);
      }
    }
  }
}