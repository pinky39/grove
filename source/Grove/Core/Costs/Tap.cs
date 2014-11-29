namespace Grove.Costs
{
  using System.Linq;

  public class Tap : Cost
  {        
    protected override void CanPay(CanPayResult result)
    {
      if (Validator != null)
      {
        result.CanPay(() => Card.Controller.Battlefield.Any(
          x => x.CanBeTapped && Validator.IsTargetValid(x, Card)));

        return;
      }

      result.CanPay(() => Card.CanTap);
    }

    public override void Pay(PayCostParameters p)
    {
      var target = p.Targets.Cost.FirstOrDefault();

      if (target != null)
      {
        target.Card().Tap();
        return;
      }

      Card.Tap();
    }
  }
}