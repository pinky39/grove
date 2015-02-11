namespace Grove.Costs
{
  using System;

  public class ChangeManaCostOfSpellsOrAbilities : CostModifier
  {
    private readonly Func<Card, CostType, CostModifier, bool> _selector;
    private ChangeManaCostOfSpellsOrAbilities() {}

    public ChangeManaCostOfSpellsOrAbilities(int amount, Func<Card, CostType, CostModifier, bool> selector)
      : base(amount)
    {
      _selector = selector ?? delegate { return true; };
    }

    protected override bool ShouldApply(Card card, CostType type)
    {
      return _selector(card, type, this);
    }
  }
}