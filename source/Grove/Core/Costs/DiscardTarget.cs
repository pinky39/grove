namespace Grove.Costs
{
  using System.Linq;

  public class DiscardTarget : Cost
  {
    public override CanPayResult CanPayPartial()
    {
      return Card.Controller.Hand.Count > 0;      
    }

    public override void PayPartial(PayCostParameters p)
    {            
      var card = p.Targets.Cost.FirstOrDefault().Card();
      card.Discard();      
    }
  }
}