namespace Grove.Core.Costs
{
  using System.Linq;
  using Targeting;

  public class Tap : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      if (Validator != null)
      {
        return Card.Controller.Battlefield.Any(
          x => x.CanBeTapped && Validator.IsTargetValid(x, Card));
      }

      return Card.CanTap;
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