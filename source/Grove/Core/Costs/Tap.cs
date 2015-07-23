namespace Grove.Costs
{
  using System.Linq;

  public class Tap : Cost
  {
    public override CanPayResult CanPayPartial(bool needsToPayManaCost)
    {
      if (Validator != null)
      {
        return Controller.Battlefield.Any(
          x => x.CanBeTapped && Validator.IsTargetValid(x, Card));
      }

      return Card.CanTap;
    }

    public override void PayPartial(PayCostParameters p)
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