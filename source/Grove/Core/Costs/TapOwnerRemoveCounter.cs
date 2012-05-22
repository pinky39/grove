namespace Grove.Core.Costs
{
  public class TapOwnerRemoveCounter : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return Card.CanTap && Card.ChargeCountersCount > 0;
    }

    public override void Pay(ITarget target, int? x)
    {
      Card.Tap();
      Card.RemoveChargeCounter();
    }
  }
}