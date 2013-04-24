namespace Grove.Ai.RepetitionRules
{
  using System.Linq;

  public abstract class RepetitionRule : MachinePlayRule
  {
    public override void Process(ActivationContext c)
    {
      if (c.HasTargets == false)
      {
        var p = new RepetitionRuleParameters(c.MaxRepetitions);
        c.Repeat = GetRepetitionCount(p);
        return;
      }

      var targetsCombinations = c.TargetsCombinations().ToList();

      foreach (var targetsCombination in targetsCombinations)
      {
        var p = new RepetitionRuleParameters(c.MaxRepetitions, targetsCombination.Targets);
        targetsCombination.Repeat = GetRepetitionCount(p);
      }
    }

    public abstract int GetRepetitionCount(RepetitionRuleParameters p);
  }
}