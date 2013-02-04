namespace Grove.Core.Ai.TimingRules
{
  using System;

  public class OwningCardHas : TimingRule
  {
    private readonly Func<Card, bool> _predicate;
    
    public OwningCardHas(Func<Card, bool> predicate)
    {
      _predicate = predicate;
    }

    public override bool ShouldPlay(TimingRuleParameters p)
    {
      return _predicate(p.Card);
    }
  }
}