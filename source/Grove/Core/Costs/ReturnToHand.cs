namespace Grove.Costs
{
  public class ReturnToHand : Cost
  {
    public override CanPayResult CanPayPartial(bool needsToPayManaCost)
    {
      return true;
    }

    public override void PayPartial(PayCostParameters p)
    {
      Card.PutToHand();
    }
  }
}