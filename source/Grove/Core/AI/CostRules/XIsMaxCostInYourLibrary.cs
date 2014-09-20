namespace Grove.AI.CostRules
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
      var cards = p.Controller.Library
        .Where(_filter)
        .ToList();

      var maxConverted = cards.Count > 0
        ? cards.Max(x => x.ConvertedCost)
        : 0;      

      return Math.Min(maxConverted, p.MaxX);
    }
  }
}