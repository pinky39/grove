namespace Grove.Core.Ai.CostRules
{
  using Mana;
  
  public class MaxAvailableMana : CostRule
  {
    private readonly ManaUsage _manaUsage;
    
    public MaxAvailableMana(ManaUsage manaUsage)
    {
      _manaUsage = manaUsage;
    }

    public override int CalculateX(CostRuleParameters p)
    {
      return p.GetMaxConvertedMana(_manaUsage);
    }
  }
}