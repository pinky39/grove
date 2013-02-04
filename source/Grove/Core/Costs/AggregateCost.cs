namespace Grove.Core.Costs
{
  using System.Collections.Generic;
  using Mana;
  using Targeting;

  public class AggregateCost : Cost
  {
    private List<Cost> Costs = new List<Cost>();

    public AggregateCost(params Cost[] costs)
    {
      Costs.AddRange(costs);
    }

    public override IManaAmount GetManaCost()
    {
      IManaAmount manaAmount = ManaAmount.Zero;

      foreach (var cost in Costs)
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

      foreach (var cost in Costs)
      {
        cost.Initialize(card, game);
      }
    }

    public override bool CanPay(ref int? maxX)
    {
      foreach (var cost in Costs)
      {
        if (!cost.CanPay(ref maxX))
          return false;
      }

      return true;
    }

    public override void Pay(Targets targets, int? x)
    {
      foreach (var cost in Costs)
      {
        cost.Pay(targets, x);
      }
    }
  }
}