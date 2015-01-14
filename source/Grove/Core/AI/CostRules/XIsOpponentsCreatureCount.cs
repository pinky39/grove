namespace Grove.AI.CostRules
{
  using System.Linq;

  public class XIsOpponentsCreatureCount : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      return p.Controller.Opponent.Battlefield.Creatures.Count();
    }
  }
}
