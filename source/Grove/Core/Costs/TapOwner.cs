namespace Grove.Costs
{
  public class TapOwner : Cost
  {
    private bool _sourceTapped;

    public override CanPayResult CanPayPartial()
    {
      return !_sourceTapped && Card.CanTap;
    }    

    protected override bool CanPayAdditionalCost()
    {
      _sourceTapped = true;
      var canPay = base.CanPayAdditionalCost();
      _sourceTapped = false;

      return canPay;
    }

    public override void PayPartial(PayCostParameters p)
    {
      Card.Tap();
    }
  }
}