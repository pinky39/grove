namespace Grove.AI.TimingRules
{
  using System;
  using System.Linq;

  public class WhenPermanentCountIs : TimingRule
  {
    private readonly int _minCount;
    private readonly Func<Card, bool> _selector;

    private WhenPermanentCountIs() {}

    public WhenPermanentCountIs(int minCount, Func<Card, bool> selector = null)
    {
      _selector = selector ?? delegate { return true; };
      _minCount = minCount;
    }

    public override bool? ShouldPlay2(TimingRuleParameters p)
    {
      return Players.Permanents().Count(x => _selector(x)) >= _minCount;
    }
  }
}