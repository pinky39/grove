namespace Grove.Gameplay.Costs
{
  using Targeting;

  public class NoCost : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay(true);
    }

    protected override void PayCost(Targets targets, int? x, int repeat) {}
  }
}