namespace Grove.Costs
{
  public class ReturnToHand : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay(true);      
    }

    protected override void PayCost(Targets targets, int? x, int repeat)
    {
      Card.PutToHand();
    }
  }
}