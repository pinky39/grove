namespace Grove.Core.Costs
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Core.Mana;
  using Grove.Core.Targeting;

  public class AggregateCost : Cost
  {
    public List<ICostFactory> CostsFactories;
    private List<Cost> _costs;

    public override IManaAmount GetManaCost()
    {
      IManaAmount manaAmount = ManaAmount.Zero;

      foreach (var cost in _costs)
      {
        var manaCost = cost.GetManaCost();

        if (manaCost != ManaAmount.Zero)
        {
          manaAmount = manaAmount.Add(cost.GetManaCost());
        }
      }

      return manaAmount;
    }

    protected override void AfterInit()
    {
      _costs = CostsFactories.Select(
        x => x.CreateCost(Card, Validator, Game)).ToList();
    }

    public override bool CanPay(ref int? maxX)
    {
      foreach (var cost in _costs)
      {
        if (!cost.CanPay(ref maxX))
          return false;
      }

      return true;
    }

    public override void Pay(ITarget target, int? x)
    {
      foreach (var cost in _costs)
      {
        cost.Pay(target, x);
      }
    }
  }
}