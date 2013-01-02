namespace Grove.Core.Cards.Costs
{
  using System.Linq;
  using Targeting;

  public class Sacrifice : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      if (Validator != null)
      {
         return Controller.Battlefield.Any(
          permanent => Validator.IsValid(permanent));
      }
      
      return Card.IsPermanent;
    }

    public override void Pay(ITarget target, int? x)
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