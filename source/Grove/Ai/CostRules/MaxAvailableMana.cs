namespace Grove.Core.Ai.CostRules
{
  public class MaxAvailableMana : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      return p.MaxX;
    }
  }
}