namespace Grove.Gameplay.AI.TimingRules
{
  using System;
  using System.Linq;

  public class WhenYourGraveyardCountIs : TimingRule
  {
    private readonly Func<Card, Game, bool> _selector;
    private int? _maxCount;
    private int? _minCount;

    private WhenYourGraveyardCountIs() {}

    public WhenYourGraveyardCountIs(Func<Card, bool> selector, int? minCount = 1, int? maxCount = null)
      : this(minCount, maxCount, (c, g) => selector(c)) {}

    public WhenYourGraveyardCountIs(int? minCount = 1, int? maxCount = null, Func<Card, Game, bool> selector = null)
    {
      _maxCount = maxCount;
      _selector = selector ?? delegate { return true; };
      _minCount = minCount;
    }

    public override bool? ShouldPlay2(TimingRuleParameters p)
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