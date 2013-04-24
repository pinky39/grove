namespace Grove.Core.Costs
{
  using Grove.Core.Targeting;

  public class NoCost : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay = true;      
    }

    protected override void Pay(ITarget target, int? x) {}
  }
}