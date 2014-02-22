namespace Grove.Gameplay.AI.TimingRules
{
  using System;

  public class WhenCardHas : TimingRule
  {
    private readonly Func<Card, bool> _predicate;

    private WhenCardHas() {}

    public WhenCardHas(Func<Card, bool> predicate)
    {
      _predicate = predicate;
    }

    public override bool? ShouldPlay1(TimingRuleParameters p)
    {
      return _predicate(p.Card);
    }
  }
}