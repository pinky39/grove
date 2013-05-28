namespace Grove.Gameplay.Costs
{
  using System;
  using Targeting;

  [Serializable]
  public class NoCost : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay = true;
    }

    protected override void Pay(ITarget target, int? x, int repeat) {}
  }
}