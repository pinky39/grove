﻿namespace Grove.Ai.TargetingRules
{
  using System;
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.Player;
  using Gameplay.Targeting;
  using Infrastructure;

  public class SelectPlayer : TargetingRule
  {
    private readonly Func<TargetingRuleParameters, Game, Player> _selector;

    public SelectPlayer(Func<TargetingRuleParameters, Game, Player> selector)
    {
      _selector = selector;
    }

    private SelectPlayer() {}

    protected override IEnumerable<Targets> SelectTargets(TargetingRuleParameters p)
    {
      var candidates = _selector(p, Game).ToEnumerable();
      return Group(candidates, 1);
    }
  }
}