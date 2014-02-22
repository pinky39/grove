namespace Grove.Gameplay.AI.CostRules
{
  using System.Linq;

  public class XIsTargetsLifepointsLeft : CostRule
  {
    public override int CalculateX(CostRuleParameters p)
    {
      if (p.Targets.Effect[0].IsPlayer())
      {
        return p.MaxX;
      }

      var lifepoints = p.Targets.Max(x => x.Life());
      return lifepoints;
    }
  }
}