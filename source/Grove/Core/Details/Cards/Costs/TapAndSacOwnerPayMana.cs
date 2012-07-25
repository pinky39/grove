namespace Grove.Core.Details.Cards.Costs
{
  using Targeting;

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