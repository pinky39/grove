namespace Grove.Costs
{
  public class DiscardRandom : Cost
  {
    public override CanPayResult CanPayPartial(bool needsToPayManaCost)
    {
      return Card.Controller.Hand.Count > 0;
    }

    public override void PayPartial(PayCostParameters p)
    {            
      var card = Card.Controller.DiscardRandomCard();
      p.Targets.AddCost(card);      
    }
  }
}