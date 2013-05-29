namespace Grove.Artifical.CostRules
{
  public class MaxAvailableMana : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      return p.MaxX;
    }
  }
}