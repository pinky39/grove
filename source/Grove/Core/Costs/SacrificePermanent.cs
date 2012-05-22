namespace Grove.Core.Costs
{
  using System.Linq;

  public class SacrificePermanent : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return Controller.Battlefield.Any(
        permanent => TargetSelector.IsValid(permanent));
    }

    public override void Pay(ITarget target, int? x)
    {
      var creature = target.Card();
      creature.Sacrifice();
    }
  }
}