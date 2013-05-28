namespace Grove.Artifical.CostRules
{
  using System;
  using System.Linq;
  using Gameplay.Targeting;

  [Serializable]
  public class TargetsLifepoints : CostRule
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