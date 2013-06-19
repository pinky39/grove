namespace Grove.Artifical.RepetitionRules
{
  using System;
  using System.Linq;

  public abstract class RepetitionRule : MachinePlayRule
  {
    public override void Process(Artifical.ActivationContext c)
    {
      if (c.HasTargets == false)
      {
        var p = new RepetitionRuleParameters(c.Card, c.MaxRepetitions);
        c.Repeat = Math.Min(p.MaxRepetitions, GetRepetitionCount(p));

        if (c.Repeat == 0)
          c.CancelActivation = true;

        return;
      }

      var targetsCombinations = c.TargetsCombinations().ToList();

      foreach (var targetsCombination in targetsCombinations)
      {
        var p = new RepetitionRuleParameters(c.Card, c.MaxRepetitions, targetsCombination.Targets);
        targetsCombination.Repeat = Math.Min(p.MaxRepetitions, GetRepetitionCount(p));

        if (targetsCombination.Repeat == 0)
        {
          c.RemoveTargetCombination(targetsCombination);
        }
      }
    }

    public abstract int GetRepetitionCount(RepetitionRuleParameters p);
  }
}