namespace Grove.Artifical.CostRules
{
  public class XIsAvailableMana : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      return p.MaxX;
    }
  }
}