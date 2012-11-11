namespace Grove.Core.Details.Cards.Costs
{
  using Mana;
  using Targeting;

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