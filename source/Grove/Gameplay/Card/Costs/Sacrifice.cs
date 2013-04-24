namespace Grove.Core.Costs
{
  using System.Linq;
  using Grove.Core.Targeting;

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

    protected override void Pay(ITarget target, int? x)
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