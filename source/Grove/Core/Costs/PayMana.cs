namespace Grove.Core.Costs
{
  using System.Linq;
  using Grove.Core.Mana;
  using Grove.Core.Targeting;

  public class PayMana : Cost
  {
    public IManaAmount Amount;    
    public ManaUsage Usage = ManaUsage.Abilities;
    public bool TryNotToConsumeCardsManaSourceWhenPayingThis;

    public override IManaAmount GetManaCost()
    {
      return Amount;
    }
    
    public override bool CanPay(ref int? maxX)
    {
      if (!Controller.HasMana(Amount, Usage))
        return false;

      if (HasX)
      {
        maxX = Controller.GetConvertedMana(Usage) - Amount.Converted;
      }

      return true;
    }

    public override void Pay(ITarget target, int? x)
    {
      var amount = Amount;

      if (x.HasValue)
      {
        amount = amount.Add(x.Value);
      }

      if (TryNotToConsumeCardsManaSourceWhenPayingThis)
      {
        var manaSource = Card.ManaSources.FirstOrDefault();
        Controller.Consume(amount, Usage, tryNotToConsumeThisSource: manaSource);
        return;
      }

      Controller.Consume(amount, Usage);
    }
  }
}