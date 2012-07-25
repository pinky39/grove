namespace Grove.Core.Details.Cards.Costs
{
  using Targeting;

  public class SacrificeOwner : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return Card.IsPermanent;
    }

    public override void Pay(Target target, int? x)
    {
      Card.Sacrifice();      
    }
  }
}