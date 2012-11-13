namespace Grove.Core.Cards.Costs
{
  using System.Linq;
  using Grove.Core.Targeting;
  using Mana;

  public class TapOwnerPayMana : Cost
  {
    public IManaAmount Amount;
    public bool HasX { get; set; }
    public bool TapOwner { get; set; }
    public bool TryNotToConsumeCardsManaSourceWhenPayingThis { get; set; }

    public override bool CanPay(ref int? maxX)
    {
      if (TapOwner && !Card.CanTap)
        return false;

      if (Amount == null)
        return true;

      if (!Controller.HasMana(Amount, ManaUsage.Abilities))
        return false;

      if (HasX)
      {
        maxX = Controller.GetConvertedMana(ManaUsage.Abilities) - Amount.Converted;
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
          amount = amount.Add(x.Value);
        }

        if (TryNotToConsumeCardsManaSourceWhenPayingThis)
        {
          var manaSource = Card.ManaSources.FirstOrDefault();
          Controller.Consume(amount, ManaUsage.Abilities, tryNotToConsumeThisSource: manaSource);
          return;
        }

        Controller.Consume(amount, ManaUsage.Abilities);
      }
    }
  }
}