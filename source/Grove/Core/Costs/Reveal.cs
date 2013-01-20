namespace Grove.Core.Costs
{
  using Targeting;

  public class Reveal : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return Card.Controller.Hand.Count > 0;
    }

    protected override void Pay(ITarget target, int? x)
    {
      target.Card().Reveal();
    }
  }
}