namespace Grove.Costs
{
  public class NoCost : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay(true);
    }

    public override void Pay(PayCostParameters p) {}
  }
}