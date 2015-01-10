namespace Grove.AI.CostRules
{
  public class XIsGraveyardCardCount : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      return p.Controller.Graveyard.Count;
    }
  }
}
