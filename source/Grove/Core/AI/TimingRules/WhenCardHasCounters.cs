namespace Grove.AI.TimingRules
{
  public class WhenCardHasCounters : TimingRule
  {
    private readonly int _minCount;
    private readonly bool _onlyAtEot;

    private WhenCardHasCounters() {}

    public WhenCardHasCounters(int minCount, bool onlyAtEot = true)
    {
      _minCount = minCount;
      _onlyAtEot = onlyAtEot;
    }

    public override bool? ShouldPlay2(TimingRuleParameters p)
    {
      if (p.Card.Counters > 0 && CanBeDestroyed(p.Card))
        return true;
      
      if (p.Card.Counters >= _minCount)
      {
        if (IsEndOfOpponentsTurn(p.Controller))
          return true;

        if (_onlyAtEot) 
          return false;

        return Stack.IsEmpty;
      }      

      return false;
    }
  }
}