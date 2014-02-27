namespace Grove.AI.RepetitionRules
{
  using System;

  public class RepeatMaxTimes : RepetitionRule
  {
    private readonly int? _max;

    private RepeatMaxTimes() {}

    public RepeatMaxTimes(int? max = null)
    {
      _max = max;
    }

    public override int GetRepetitionCount(RepetitionRuleParameters p)
    {
      if (_max.HasValue)
        return Math.Min(_max.Value, p.MaxRepetitions);

      return p.MaxRepetitions;
    }
  }
}