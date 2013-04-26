namespace Grove.Ai.TimingRules
{
  using Gameplay.States;

  public class ChargeCounters : TimingRule
  {
    private readonly int _minCount;
    private readonly bool _onlyAtEot;

    private ChargeCounters() {}

    public ChargeCounters(int minCount, bool onlyAtEot = true)
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