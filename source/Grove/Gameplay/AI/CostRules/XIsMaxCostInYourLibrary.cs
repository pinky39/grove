namespace Grove.Gameplay.AI.CostRules
{
  using System;
  using System.Linq;

  public class XIsMaxCostInYourLibrary : CostRule
  {
    private readonly Func<Card, bool> _filter;

    private XIsMaxCostInYourLibrary() {}

    public XIsMaxCostInYourLibrary(Func<Card, bool> filter)
    {
      _filter = filter;
    }

    public override int CalculateX(CostRuleParameters p)
    {
      var maxConverted = p.Controller.Library.Where(_filter)
        .Max(x => x.ConvertedCost);

      return Math.Min(maxConverted, p.MaxX);
    }
  }
}