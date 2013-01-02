namespace Grove.Core.Cards.Costs
{
  using Targeting;

  public class Discard : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return true;
    }

    public override void Pay(ITarget target, int? x)
    {
      Card.Discard();
    }
  }
}