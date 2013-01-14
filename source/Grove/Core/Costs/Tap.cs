namespace Grove.Core.Costs
{
  using System.Linq;
  using Grove.Core.Targeting;

  public class Tap : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      if (Validator != null)
      {        
        return Controller.Battlefield.Any(
          x => x.CanBeTapped && Validator.IsValid(x));
      }
      
      return Card.CanTap;
    }

    public override void Pay(ITarget target, int? x)
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