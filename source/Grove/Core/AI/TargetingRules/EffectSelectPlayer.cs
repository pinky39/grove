﻿namespace Grove.AI.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using Grove.Infrastructure;

  public class EffectSelectPlayer : TargetingRule
  {
    private readonly Func<TargetingRuleParameters, Game, Player> _selector;

    public EffectSelectPlayer(Func<TargetingRuleParameters, Game, Player> selector)
    {
      _selector = selector;
    }

    private EffectSelectPlayer() {}

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = _selector(p, Game).ToEnumerable();
      return Group(candidates, 1);
    }
  }
}