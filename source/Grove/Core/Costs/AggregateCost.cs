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

    public override void Initialize(Card card, Game game)
    {
      base.Initialize(card, game);

      foreach (var cost in _costs)
      {
        cost.Initialize(card, game);
      }
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

    public override void Pay(Targets targets, int? x)
    {
      foreach (var cost in _costs)
      {
        cost.Pay(targets, x);
      }
    }
  }
}