namespace Grove.Costs
{
  public class TapOwner : Cost
  {
    protected override void CanPay(CanPayResult result)
    {     
      result.CanPay(() => Card.CanTap);
    }

    public override void Pay(PayCostParameters p)
    {
      Card.Tap();
    }
  }
}