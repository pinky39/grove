namespace Grove.AI.TargetingRules
{
    using System;
    using System.Collections.Generic;
  using System.Linq;

  public class EffectFight : TargetingRule
  {
    private readonly Func<Card, int> _selector;

    private EffectFight()
    {
    }

    public EffectFight(Func<Card, int> selector)
    {
      _selector = selector;
    }
    
    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var yours = p.Candidates<Card>(selectorIndex: 0)
        .OrderBy(c => -_selector(c))
        .FirstOrDefault();

      if (yours == null)
        return None<Targets>();

      var yoursValue = _selector(yours);

      var opponents = p.Candidates<Card>(selectorIndex: 1)
        .Where(c => _selector(c) <= yoursValue)
        .OrderBy(c => -c.Score);

      var opponentCandidates = p.Candidates<Card>(selectorIndex: 1)
        .OrderBy(c => c.Toughness);

      return Group(new[] { yours }, opponentCandidates,
        (t, tgs) => tgs.AddEffect(t), (t, tgs) => tgs.AddEffect(t));
    }
  }
}
