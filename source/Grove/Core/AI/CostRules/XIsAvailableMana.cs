namespace Grove.AI.CostRules
{
  public class XIsAvailableMana : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      return p.MaxX;
    }
  }
}