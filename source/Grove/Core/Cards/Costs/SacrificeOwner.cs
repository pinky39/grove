namespace Grove.Core.Details.Cards.Costs
{
  using Targeting;

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