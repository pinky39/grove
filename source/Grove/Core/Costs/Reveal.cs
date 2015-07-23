namespace Grove.Costs
{
  using System.Linq;

  public class Reveal : Cost
  {
    public override CanPayResult CanPayPartial(bool needsToPayManaCost)
    {
      return Card.Controller.Hand.Count > 0;      
    }

    public override void PayPartial(PayCostParameters p)
    {      
      var card = p.Targets.Cost.FirstOrDefault().Card();
      card.Reveal();
    }
  }
}