namespace Grove.Core.Cards.Costs
{
  using Grove.Core.Targeting;

  public class SacrificeOwner : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return Card.IsPermanent();
    }

    public override void Pay(ITarget target, int? x)
    {
      Card.Sacrifice();
    }
  }
}