namespace Grove.Core.Ai.TimingRules
{
  using System;

  public class OwningCardHas : TimingRule
  {
    public Func<Card, bool> Predicate;

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return Predicate(p.Card);
    }
  }
}