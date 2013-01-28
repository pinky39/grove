namespace Grove.Core.Ai.CostRules
{
  using System.Linq;
  using Targeting;

  public class TargetsLifepoints : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      var lifepoints = p.Targets.Max(x => x.Life());
      return lifepoints;
    }
  }
}