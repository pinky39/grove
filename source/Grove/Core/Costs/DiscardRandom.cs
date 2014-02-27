namespace Grove.Costs
{
  public class DiscardRandom : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay(() => Card.Controller.Hand.Count > 0);
    }

    protected override void PayCost(Targets targets, int? x, int repeat)
    {
      var card = Card.Controller.DiscardRandomCard();
      targets.AddCost(card);
    }
  }
}