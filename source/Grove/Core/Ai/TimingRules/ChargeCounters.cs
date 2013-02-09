namespace Grove.Core.Ai.TimingRules
{
  public class ChargeCounters : TimingRule
  {
    private readonly int _minCount;

    public ChargeCounters(int minCount)
    {
      _minCount = minCount;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (Turn.Step == Step.EndOfTurn && p.Card.Counters >= _minCount)
        return true;

      if (p.Card.Counters > 0 && CanBeDestroyed(p))
        return true;

      return false;
    }
  }
}