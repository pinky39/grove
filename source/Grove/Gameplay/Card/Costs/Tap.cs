namespace Grove.Gameplay.Card.Costs
{
  using System.Linq;
  using Targeting;

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

    protected override void Pay(ITarget target, int? x)
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