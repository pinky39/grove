namespace Grove.Gameplay.AI.CostRules
{
  public class XIsAvailableMana : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      return p.MaxX;
    }
  }
}