namespace Grove.Artifical.TimingRules
{
  public class OpponentHandCountIs : TimingRule
  {
    private readonly int? _maxCount;
    private readonly int _minCount;

    private OpponentHandCountIs() {}

    public OpponentHandCountIs(int? minCount = null, int? maxCount = null)
    {
      _minCount = minCount ?? 0;
      _maxCount = maxCount;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      var opponent = p.Controller.Opponent;

      if (opponent.Hand.Count < _minCount)
        return false;

      if (_maxCount.HasValue && opponent.Hand.Count > _maxCount)
        return false;

      return true;
    }
  }
}