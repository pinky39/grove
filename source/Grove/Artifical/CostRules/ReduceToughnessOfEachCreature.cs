namespace Grove.Artifical.CostRules
{
  public class ReduceToughnessOfEachCreature : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      const int maxXToTry = 6;
      return MassRemovalParameterOptimizer.CalculateOptimalDamage(p.Controller, p.Controller.Opponent, maxXToTry);      
    }
  }
}