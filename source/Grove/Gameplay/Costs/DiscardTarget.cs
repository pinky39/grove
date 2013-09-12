namespace Grove.Gameplay.Costs
{
  using System.Linq;
  using Targeting;

  public class DiscardTarget : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay(() => Card.Controller.Hand.Count > 0);
    }

    protected override void PayCost(Targets targets, int? x, int repeat)
    {
      var card = targets.Cost.FirstOrDefault().Card();
      card.Discard();
    }
  }
}