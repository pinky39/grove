namespace Grove.Core.Cards.Costs
{
  using System.Linq;
  using Grove.Core.Targeting;

  public class SacPermanent : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return Controller.Battlefield.Any(
        permanent => Validator.IsValid(permanent));
    }

    public override void Pay(ITarget target, int? x)
    {
      var creature = target.Card();
      creature.Sacrifice();
    }
  }
}