namespace Grove.Artifical.TimingRules
{
  using System;
  using System.Linq;
  using Gameplay;

  public class ControllerGraveyardCountIs : TimingRule
  {
    private readonly Func<Card, Game, bool> _selector;
    private int? _maxCount;
    private int? _minCount;

    private ControllerGraveyardCountIs() {}

    public ControllerGraveyardCountIs(Func<Card, bool> selector, int? minCount = 1, int? maxCount = null)
      : this(minCount, maxCount, (c, g) => selector(c)) {}

    public ControllerGraveyardCountIs(int? minCount = 1, int? maxCount = null, Func<Card, Game, bool> selector = null)
    {
      _maxCount = maxCount;
      _selector = selector ?? delegate { return true; };
      _minCount = minCount;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      var result = true;

      var count = p.Controller.Graveyard.Count(c => _selector(c, Game));

      if (_minCount.HasValue)
        result = count >= _minCount;

      if (_maxCount.HasValue)
        result = result && count <= _maxCount;

      return result;
    }
  }
}