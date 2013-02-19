namespace Grove.Core.Ai.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using Infrastructure;
  using Targeting;

  public class SelectPlayer : TargetingRule
  {
    private Func<TargetingRuleParameters, Game, Player> _selector;
    
    public SelectPlayer(Func<TargetingRuleParameters, Game, Player> selector)
    {
      _selector = selector;
    }

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = _selector(p, Game).ToEnumerable();
      return Group(candidates, 1);
    }
  }
}