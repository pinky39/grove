namespace Grove.Artifical.TimingRules
{
  using System;
  using System.Linq;
  using Gameplay;

  [Serializable]
  public class ControllerGravayardCountIs : TimingRule
  {
    private readonly Func<Card, bool> _selector;
    private int? _maxCount;
    private int? _minCount;

    private ControllerGravayardCountIs() {}

    public ControllerGravayardCountIs(int? minCount = 1, int? maxCount = null, Func<Card, bool> selector = null)
    {
      _maxCount = maxCount;
      _selector = selector ?? delegate { return true; };
      _minCount = minCount;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      var result = true;

      if (_minCount.HasValue)
        result = p.Controller.Graveyard.Count(_selector) >= _minCount;

      if (_maxCount.HasValue)
        result = result && p.Controller.Graveyard.Count(_selector) <= _maxCount;

      return result;
    }
  }
}