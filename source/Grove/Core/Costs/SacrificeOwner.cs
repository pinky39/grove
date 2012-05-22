namespace Grove.Core.Costs
{
  public class SacrificeOwner : Cost
  {      
    public override bool CanPay(ref int? maxX)
    {
      return true;
    }

    public override void Pay(ITarget target, int? x)
    {
      Controller.SacrificeCard(Ability.OwningCard);
    }    
  }
}