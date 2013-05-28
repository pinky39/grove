namespace Grove.Artifical.RepetitionRules
{
  using System;

  [Serializable]
  public class MaxRepetitions : RepetitionRule
  {
    private readonly int? _max;

    private MaxRepetitions() {}

    public MaxRepetitions(int? max = null)
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