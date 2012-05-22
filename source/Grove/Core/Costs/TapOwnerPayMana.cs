namespace Grove.Core.Costs
{
  using System.Linq;

  public class TapOwnerPayMana : Cost
  {
    public ManaAmount Amount;
    public bool HasX { get; set; }
    public bool TapOwner { get; set; }
    public bool TryNotToConsumeCardsManaSourceWhenPayingThis { get; set; }

    public override bool CanPay(ref int? maxX)
    {
      if (TapOwner && !Card.CanTap)
        return false;

      if (Amount == null)
        return true;

      if (!Controller.HasEnoughMana(Amount))
        return false;

      if (HasX)
      {
        maxX = Controller.GetMaxConvertedMana() - Amount.Converted;
      }

      return true;
    }

    public override void Pay(ITarget target, int? x)
    {
      if (TapOwner)
        Card.Tap();

      if (Amount != null)
      {
        var amount = Amount;

        if (x.HasValue)
        {
          amount = amount + ManaAmount.Colorless(x.Value);
        }

        if (TryNotToConsumeCardsManaSourceWhenPayingThis)
        {
          var manaSource = Card.ManaSources.FirstOrDefault();
          Controller.Consume(amount, tryNotToConsumeThisSource: manaSource);
          return;
        }

        Controller.Consume(amount);
      }
    }
  }
}