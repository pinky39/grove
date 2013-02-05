namespace Grove.Core.Ai.TimingRules
{
  using System;
  using System.Linq;

  public class OpponentHasPermanents : TimingRule
  {
    private readonly Func<Card, bool> _selector;
    private readonly int _minCount;

    public OpponentHasPermanents(Func<Card, bool> selector = null, int minCount = 1)
    {
      _minCount = minCount;
      _selector = selector ?? delegate { return true; };
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Players.Permanents().Count(x => _selector(x) && x.Controller != p.Controller) >= _minCount;
    }
  }
}