namespace Grove.Artifical.CostRules
{
  using System;

  [Serializable]
  public class MaxAvailableMana : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      return p.MaxX;
    }
  }
}