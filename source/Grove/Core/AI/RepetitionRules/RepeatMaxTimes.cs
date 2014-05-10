namespace Grove.AI.RepetitionRules
{
  using System;

  public class RepeatMaxTimes : RepetitionRule
  {
    private readonly Func<RepetitionRuleParameters, int?> _max;

    private RepeatMaxTimes() {}

    public RepeatMaxTimes(Func<RepetitionRuleParameters, int?> max)
    {
      _max = max;
    }
    
    public RepeatMaxTimes(int? max = null)
    {
      _max = delegate { return max; };
    }

    public override int GetRepetitionCount(RepetitionRuleParameters p)
    {
      var max = _max(p);
      
      return max.HasValue 
        ? Math.Min(max.Value, p.MaxRepetitions) 
        : p.MaxRepetitions;
    }
  }
}