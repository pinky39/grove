namespace Grove.Gameplay.Costs
{
  using System;
  using System.Linq;
  using Targeting;

  [Serializable]
  public class Tap : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      if (Validator != null)
      {
        result.CanPay = Card.Controller.Battlefield.Any(
          x => x.CanBeTapped && Validator.IsTargetValid(x, Card));

        return;
      }

      result.CanPay = Card.CanTap;
    }

    protected override void Pay(ITarget target, int? x, int repeat)
    {
      if (target != null)
      {
        target.Card().Tap();
        return;
      }

      Card.Tap();
    }
  }
}