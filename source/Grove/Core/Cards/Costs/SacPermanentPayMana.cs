namespace Grove.Core.Cards.Costs
{
  using Grove.Core.Targeting;
  using Mana;

  public class SacPermanentPayMana : SacPermanent
  {
    public IManaAmount Amount;
    
    public override bool CanPay(ref int? maxX)
    {
      return Controller.HasMana(Amount, ManaUsage.Abilities) && 
        base.CanPay(ref maxX);
    }

    public override void Pay(ITarget target, int? x)
    {      
      Controller.Consume(Amount, ManaUsage.Abilities);
      base.Pay(target, x);
    }
  }
}