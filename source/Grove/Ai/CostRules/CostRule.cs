namespace Grove.Ai.CostRules
{
  using System.Linq;

  public abstract class CostRule : MachinePlayRule
  {
    public override void Process(ActivationContext c)
    {
      if (c.HasTargets == false)
      {        
        var p = new CostRuleParameters(c.Card, c.MaxX.GetValueOrDefault());
        c.X = CalculateX(p);

        if (c.X > c.MaxX)
        {
          c.CancelActivation = true;
        }
        return;
      }

      var targetsCombinations = c.TargetsCombinations().ToList();
      
      foreach (var targetsCombination in targetsCombinations) {
        var p = new CostRuleParameters(c.Card, c.MaxX.GetValueOrDefault(), targetsCombination.Targets);
        targetsCombination.X = CalculateX(p);
        
        if (targetsCombination.X > c.MaxX)
        {
          c.RemoveTargetCombination(targetsCombination);
        }
      }

      if (c.HasTargets == false)
        c.CancelActivation = true;
    }

    public abstract int CalculateX(CostRuleParameters p);
  }
}