namespace Grove.AI.RepetitionRules
{
  using System;

  public class RepeatForEachLifepointTargetHasLeft : RepetitionRule
  {
    public override int GetRepetitionCount(RepetitionRuleParameters p)
    {
      if (p.Targets.Effect[0].IsPlayer())
      {
        return p.MaxRepetitions;
      }

      var lifepoints = p.Targets.Effect[0].Life();
      return Math.Min(lifepoints, p.MaxRepetitions);
    }
  }
}