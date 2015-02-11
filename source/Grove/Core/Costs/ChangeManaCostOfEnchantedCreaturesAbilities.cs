namespace Grove.Costs
{
  public class ChangeManaCostOfEnchantedCreaturesAbilities : CostModifier
  {
    private ChangeManaCostOfEnchantedCreaturesAbilities() {}

    public ChangeManaCostOfEnchantedCreaturesAbilities(int amount) : base(amount) {}
    
    protected override bool ShouldApply(Card card, CostType type)
    {
      if (type != CostType.Ability)
        return false;

      return card == Source.AttachedTo;
    }
  }
}