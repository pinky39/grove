namespace Grove.AI.CostRules
{
  public class XIsOptimalDamage : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      const int maxXToTry = 6;
      return MassRemovalParameterOptimizer.CalculateOptimalDamage(p.Controller, p.Controller.Opponent, maxXToTry);      
    }
  }
}