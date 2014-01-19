namespace Grove.Gameplay.Costs
{
  using Targeting;

  public class TapOwner : Cost
  {
    protected override void CanPay(CanPayResult result)
    {     
      result.CanPay(() => Card.CanTap);
    }

    protected override void PayCost(Targets targets, int? x, int repeat)
    {
      Card.Tap();
    }
  }
}