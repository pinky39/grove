namespace Grove.Artifical.TimingRules
{
  using System;
  using Gameplay;

  public class WhenCardHas : TimingRule
  {
    private readonly Func<Card, bool> _predicate;

    private WhenCardHas() {}

    public WhenCardHas(Func<Card, bool> predicate)
    {
      _predicate = predicate;
    }

    public override bool? ShouldPlay2(TimingRuleParameters p)
    {
      return _predicate(p.Card);
    }
  }
}