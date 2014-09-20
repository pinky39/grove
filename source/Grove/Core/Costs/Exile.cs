namespace Grove.Costs
{
  using System.Linq;

  public class Exile : Cost
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

    protected override void PayCost(Targets targets, int? x, int repeat)
    {
      var target = targets.Cost.FirstOrDefault();

      if (target != null)
      {
        target.Card().Exile();
        return;
      }

      Card.Exile();
    }
  }
}