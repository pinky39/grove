namespace Grove.Costs
{
  using System.Linq;

  public class DiscardTarget : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay(() => Card.Controller.Hand.Count > 0);
    }

    public override void Pay(PayCostParameters p)
    {
      var card = p.Targets.Cost.FirstOrDefault().Card();
      card.Discard();
    }
  }
}