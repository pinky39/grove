namespace Grove.Core.Details.Cards.Costs
{
  using Targeting;

  public class NoCost : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return true;
    }

    public override void Pay(Target target, int? x)
    {      
    }
  }
}