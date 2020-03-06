namespace Grove.Costs
{
  using System.Linq;
  public class ExileOwnerCost : Cost
  {        
    public ExileOwnerCost()
    {      
    }

    public override CanPayResult CanPayPartial(bool needsToPayManaCost)
    {
      return true;      
    }

    public override void PayPartial(PayCostParameters p)
    {      
      Card.Exile();
    }
  }
}