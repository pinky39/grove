namespace Grove.Gameplay.Costs
{
  using System;
  using Targeting;

  [Serializable]
  public class DiscardTarget : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      result.CanPay = Card.Controller.Hand.Count > 0;
    }

    protected override void Pay(ITarget target, int? x, int repeat)
    {
      target.Card().Discard();
    }
  }
}