namespace Grove.Core.Costs
{
  using System.Linq;
  using Mana;
  using Targeting;

  public class PayMana : Cost
  {
    private readonly IManaAmount _amount;
    private readonly bool _hasX;
    private readonly ManaUsage _manaUsage;
    private readonly bool _tryNotToConsumeCardsManaSourceWhenPayingThis;

    private PayMana() {}

    public PayMana(IManaAmount amount, ManaUsage manaUsage, bool hasX = false,
      bool tryNotToConsumeCardsManaSourceWhenPayingThis = false)
    {
      _amount = amount;
      _manaUsage = manaUsage;
      _hasX = hasX;
      _tryNotToConsumeCardsManaSourceWhenPayingThis = tryNotToConsumeCardsManaSourceWhenPayingThis;
    }

    public override bool HasX { get { return _hasX; } }

    public override IManaAmount GetManaCost()
    {
      return _amount;
    }

    public override bool CanPay(ref int? maxX)
    {
      if (!Card.Controller.HasMana(_amount, _manaUsage))
        return false;

      if (_hasX)
      {
        maxX = Card.Controller.GetConvertedMana(_manaUsage) - _amount.Converted;
      }

      return true;
    }

    protected override void Pay(ITarget target, int? x)
    {
      var amount = _amount;

      if (x.HasValue)
      {
        amount = amount.Add(x.Value);
      }

      if (_tryNotToConsumeCardsManaSourceWhenPayingThis)
      {
        var manaSource = Card.ManaSources.FirstOrDefault();
        Card.Controller.Consume(amount, _manaUsage, tryNotToConsumeThisSource: manaSource);
        return;
      }

      Card.Controller.Consume(amount, _manaUsage);
    }
  }
}