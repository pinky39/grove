namespace Grove.Artifical.TimingRules
{
  using Gameplay.States;

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

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      if (p.Card.Counters >= _minCount)
      {
        if (_onlyAtEot && (p.Controller.IsActive || Turn.Step != Step.EndOfTurn))
          return false;

        return true;
      }

      if (p.Card.Counters > 0 && CanBeDestroyed(p))
        return true;

      return false;
    }
  }
}