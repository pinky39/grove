namespace Grove.Gameplay.Costs
{
  using System;
  using System.Linq;
  using Targeting;

  [Serializable]
  public class Sacrifice : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      if (Validator != null)
      {
        result.CanPay = Card.Controller.Battlefield.Any(
          permanent => Validator.IsTargetValid(permanent, Card));

        return;
      }

      result.CanPay = Card.IsPermanent;
    }

    protected override void Pay(ITarget target, int? x, int repeat)
    {
      if (target != null)
      {
        target.Card().Sacrifice();
        return;
      }

      Card.Sacrifice();
    }
  }
}