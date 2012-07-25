namespace Grove.Core.Details.Cards.Costs
{
  using Targeting;

  public class RevealCardFromHand : Cost
  {
    public override bool CanPay(ref int? maxX)
    {
      return Controller.Hand.Count > 0;
    }

    public override void Pay(Target target, int? x)
    {
      target.Card().Reveal();
    }
  }
}