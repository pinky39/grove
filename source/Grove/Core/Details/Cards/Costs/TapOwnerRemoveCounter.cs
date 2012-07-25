namespace Grove.Core.Details.Cards.Costs
{
  using Targeting;

  public class TapOwnerRemoveCounter : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return Card.CanTap && Card.ChargeCountersCount > 0;
    }

    public override void Pay(Target target, int? x)
    {
      Card.Tap();
      Card.RemoveChargeCounter();
    }
  }
}