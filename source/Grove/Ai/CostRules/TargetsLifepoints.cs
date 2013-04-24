namespace Grove.Core.Ai.CostRules
{
  using System.Linq;
  using Targeting;

  public class TargetsLifepoints : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {            
      if (p.Targets.Effect[0].IsPlayer())
      {
        return p.MaxX;
      }
      
      var lifepoints = p.Targets.Max(x => x.Life());
      return lifepoints;
    }
  }
}