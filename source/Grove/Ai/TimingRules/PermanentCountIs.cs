namespace Grove.Ai.TimingRules
{
  using System;
  using System.Linq;
  using Core;
  using Gameplay.Card;

  public class PermanentCountIs : TimingRule
  {
    private readonly int _minCount;
    private readonly Func<Card, bool> _selector;

    private PermanentCountIs() {}

    public PermanentCountIs(Func<Card, bool> selector = null, int minCount = 1)
    {
      _selector = selector ?? delegate { return true; };
      _minCount = minCount;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Players.Permanents().Count(x => _selector(x)) >= _minCount;
    }
  }
}