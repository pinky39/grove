namespace Grove.Core.Details.Cards.Costs
{
  using System.Linq;
  using Targeting;

  public class SacrificeCreature : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return Controller.Battlefield.Count(x => x.Is().Creature) > 0;
    }

    public override void Pay(ITarget target, int? x)
    {
      target.Card().Sacrifice();
    }
  }
}