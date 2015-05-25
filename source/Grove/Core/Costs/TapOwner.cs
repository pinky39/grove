namespace Grove.Costs
{
  public class TapOwner : Cost
  {    
    public override CanPayResult CanPayPartial()
    {
      return Card.CanTap;
    }       

    public override void PayPartial(PayCostParameters p)
    {
      Card.Tap();
    }
  }
}