namespace Grove.Gameplay.Costs
{
  using Targeting;

  public class Discard : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay = true;
    }

    protected override void Pay(ITarget target, int? x, int repeat)
    {
      Card.Discard();
    }
  }
}