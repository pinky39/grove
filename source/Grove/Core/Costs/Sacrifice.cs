namespace Grove.Core.Costs
{
  using System.Linq;
  using Targeting;

  public class Sacrifice : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      if (Validator != null)
      {
        return Card.Controller.Battlefield.Any(
          permanent => Validator.IsTargetValid(permanent, Card));
      }

      return Card.IsPermanent;
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