namespace Grove.Core.Ai.TimingRules
{
  using System;
  using System.Linq;

  public class ControllerGravayardCountIs : TimingRule
  {
    private readonly Func<Card, bool> _selector;
    private int? _maxCount;
    private int? _minCount;

    private ControllerGravayardCountIs() {}

    public ControllerGravayardCountIs(int? minCount = 1, int? maxCount = 1, Func<Card, bool> selector = null)
    {
      _maxCount = maxCount;
      _selector = selector ?? delegate { return true; };
      _minCount = minCount;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      var result = true;

      if (_minCount.HasValue)
        result = result && p.Controller.Graveyard.Count(_selector) >= _minCount;

      if (_maxCount.HasValue)
        result = result && p.Controller.Graveyard.Count(_selector) <= _maxCount;

      return result;
    }
  }
}