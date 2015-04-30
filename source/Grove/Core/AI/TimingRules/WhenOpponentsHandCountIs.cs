namespace Grove.AI.TimingRules
{
  public class WhenOpponentsHandCountIs : TimingRule
  {
    private readonly int? _maxCount;
    private readonly int _minCount;

    private WhenOpponentsHandCountIs() {}

    public WhenOpponentsHandCountIs(int? minCount = null, int? maxCount = null)
    {
      _minCount = minCount ?? 0;
      _maxCount = maxCount;
    }

    public override bool ShouldPlayAfterTargets(TimingRuleParameters p)
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