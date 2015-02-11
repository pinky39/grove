namespace Grove.Costs
{
  public class DiscardThis : Cost
  {
    public override CanPayResult CanPayPartial()
    {
      return true;
    }

    public override void PayPartial(PayCostParameters p)
    {      
      Card.Discard(); 
    }
  }
}