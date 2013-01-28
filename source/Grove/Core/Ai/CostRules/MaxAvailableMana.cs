namespace Grove.Core.Ai.CostRules
{
  using Mana;
  
  public class MaxAvailableMana : CostRule
  {
    public ManaUsage ManaUsage = ManaUsage.Spells;
    
    public override int CalculateX(CostRuleParameters p)
    {
      return p.GetMaxConvertedMana(ManaUsage);
    }
  }
}