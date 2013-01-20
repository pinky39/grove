namespace Grove.Core.Costs
{
  using System.Linq;
  using Mana;
  using Targeting;

  public class PayMana : Cost
  {
    public IManaAmount Amount;
    public bool HasX;
    public bool TryNotToConsumeCardsManaSourceWhenPayingThis;
    public ManaUsage Usage = ManaUsage.Abilities;

    public override IManaAmount GetManaCost()
    {
      return Amount;
    }

    public override bool CanPay(ref int? maxX)
    {
      if (!Card.Controller.HasMana(Amount, Usage))
        return false;

      if (HasX)
      {
        maxX = Card.Controller.GetConvertedMana(Usage) - Amount.Converted;
      }

      return true;
    }

    protected override void Pay(ITarget target, int? x)
    {
      var amount = Amount;

      if (x.HasValue)
      {
        amount = amount.Add(x.Value);
      }

      if (TryNotToConsumeCardsManaSourceWhenPayingThis)
      {
        var manaSource = Card.ManaSources.FirstOrDefault();
        Card.Controller.Consume(amount, Usage, tryNotToConsumeThisSource: manaSource);
        return;
      }

      Card.Controller.Consume(amount, Usage);
    }
  }
}