namespace Grove.Core.Details.Cards.Costs
{
  using Targeting;

  public class DiscardOwnerPayMana : TapOwnerPayMana
  {
    public override void Pay(ITarget target, int? x)
    {
      Card.Discard();

      base.Pay(target, x);
    }
  }
}