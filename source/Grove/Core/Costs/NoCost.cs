namespace Grove.Core.Costs
{
  using Grove.Core.Targeting;

  public class NoCost : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return true;
    }

    public override void Pay(ITarget target, int? x)
    {      
    }
  }
}