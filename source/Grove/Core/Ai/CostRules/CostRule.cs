namespace Grove.Core.Ai.CostRules
{
  using System.Linq;

  public abstract class CostRule : MachinePlayRule
  {
    public override void Process(ActivationContext c)
    {
      if (c.HasTargets == false)
      {
        var p = new CostRuleParameters(c.Card);
        c.X = CalculateX(p);
        return;
      }

      var targetsCombinations = c.TargetsCombinations().ToList();
      
      for (var i = 0; i < targetsCombinations.Count; i++)
      {
        var targetsCombination = targetsCombinations[i];
        var p = new CostRuleParameters(c.Card, targetsCombination.Targets);
        targetsCombination.X = CalculateX(p);
      }
    }

    public abstract int CalculateX(CostRuleParameters p);
  }
}