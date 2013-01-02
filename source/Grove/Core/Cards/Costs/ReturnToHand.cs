namespace Grove.Core.Cards.Costs
{
  using Targeting;

  public class ReturnToHand : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return true;
    }

    public override void Pay(ITarget target, int? x)
    {
      Card.PutToHand();
    }
  }
}