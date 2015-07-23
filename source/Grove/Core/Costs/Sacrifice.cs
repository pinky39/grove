namespace Grove.Costs
{
  using System.Linq;

  public class Sacrifice : Cost
  {
    public override CanPayResult CanPayPartial(bool needsToPayManaCost)
    {
      if (Validator != null)
      {
        return
          Controller.Battlefield.Any(
            permanent => Validator.IsTargetValid(permanent, Card));
      }

      return Card.IsPermanent;
    }

    public override void PayPartial(PayCostParameters p)
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