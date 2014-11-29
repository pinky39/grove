namespace Grove.Costs
{
  public class DiscardRandom : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay(() => Card.Controller.Hand.Count > 0);
    }

    public override void Pay(PayCostParameters p)
    {
      var card = Card.Controller.DiscardRandomCard();
      p.Targets.AddCost(card);
    }
  }
}