namespace Grove.Core.Cards.Costs
{
  using Grove.Core.Targeting;

  public class DiscardOwnerPayMana : TapOwnerPayMana
  {
    public override void Pay(ITarget target, int? x)
    {
      Card.Discard();

      base.Pay(target, x);
    }
  }
}