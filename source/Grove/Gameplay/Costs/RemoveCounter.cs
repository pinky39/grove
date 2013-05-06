namespace Grove.Gameplay.Costs
{
  using Targeting;

  public class RemoveCounter : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay = Card.CountersCount > 0;
    }

    protected override void Pay(ITarget target, int? x)
    {
      Card.RemoveChargeCounter();
    }
  }
}