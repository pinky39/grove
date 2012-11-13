namespace Grove.Core.Cards.Costs
{
  using Grove.Core.Targeting;

  public class TapAndSacOwnerPayMana : TapOwnerPayMana
  {        
    public TapAndSacOwnerPayMana()
    {
      TapOwner = true;  
    }
    
    public override void Pay(ITarget target, int? x)
    {
      base.Pay(target, x);
      Card.Sacrifice();
    }
  }
}