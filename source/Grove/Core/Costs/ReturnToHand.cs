namespace Grove.Core.Costs
{
  using Targeting;

  public class ReturnToHand : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return true;
    }

    protected override void Pay(ITarget target, int? x)
    {
      Card.PutToHand();
    }
  }
}