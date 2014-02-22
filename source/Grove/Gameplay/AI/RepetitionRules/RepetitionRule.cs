namespace Grove.Gameplay.AI.RepetitionRules
{
  using System;
  using System.Linq;
  using ActivationContext = AI.ActivationContext;

  public abstract class RepetitionRule : MachinePlayRule
  {
    public override bool Process(int pass, ActivationContext c)
    {
      if (pass == 2)
      {
        Process(c);
        return true;
      }

      return false;
    }
    
    public void Process(ActivationContext c)
    {
      if (c.HasTargets == false)
      {
        var p = new RepetitionRuleParameters(c.Card, c.MaxRepetitions.Value);
        c.Repeat = Math.Min(p.MaxRepetitions, GetRepetitionCount(p));

        if (c.Repeat == 0)
          c.CancelActivation = true;

        return;
      }

      var targetsCombinations = c.TargetsCombinations().ToList();

      foreach (var targetsCombination in targetsCombinations)
      {
        var p = new RepetitionRuleParameters(c.Card, c.MaxRepetitions.Value, targetsCombination.Targets);
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