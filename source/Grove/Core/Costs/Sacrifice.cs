namespace Grove.Costs
{
  using System.Linq;

  public class Sacrifice : Cost
  {
    protected override void CanPay(CanPayResult result)
    {
      if (Validator != null)
      {
        result.CanPay(Card.Controller.Battlefield.Any(
          permanent => Validator.IsTargetValid(permanent, Card)));

        return;
      }

      result.CanPay(() => Card.IsPermanent);
    }

    public override void Pay(PayCostParameters p)
    {
      var target = p.Targets.Cost.FirstOrDefault();

      if (target != null)
      {
        target.Card().Sacrifice();
        return;
      }

      Card.Sacrifice();
    }
  }
}