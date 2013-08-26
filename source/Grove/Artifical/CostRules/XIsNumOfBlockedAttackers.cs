namespace Grove.Artifical.CostRules
{
  using System.Linq;

  public class XIsNumOfBlockedAttackers : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      return p.Controller.Battlefield.Creatures.Count(x => x.HasBlockers);
    }
  }
}