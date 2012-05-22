namespace Grove.Core.Costs
{
  using System.Linq;

  public class TapCreature : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return Controller.Battlefield.Any(
        x => x.CanBeTapped && x.Is().Creature);
    }

    public override void Pay(ITarget target, int? x)
    {      
      target.Card().Tap();
    }        
  }
}