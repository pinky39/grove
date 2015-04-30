namespace Grove.AI.TimingRules
{
  using System;
  using System.Linq;

  public class WhenYouHavePermanents : TimingRule
  {
    private readonly int _minCount;
    private readonly Func<Card, bool> _selector;

    private WhenYouHavePermanents() {}

    public WhenYouHavePermanents(Func<Card, bool> selector = null, int minCount = 1)
    {
      _selector = selector ?? delegate { return true; };
      _minCount = minCount;
    }

    public override bool ShouldPlayAfterTargets(TimingRuleParameters p)
    {
      return Players.Permanents().Count(x => _selector(x) && x.Controller == p.Controller) >= _minCount;
    }
  }
}