﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;

  public class GoblinWarBuggy : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Goblin War Buggy")
        .ManaCost("{1}{R}")
        .Type("Creature Goblin")
        .Text(
          "{Haste}{EOL}{Echo} {1}{R} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.)")
        .Power(2)
        .Toughness(2)
        .Cast(p => p.TimingRule(new FirstMain()))
        .Echo("{1}{R}")
        .StaticAbilities(Static.Haste);
    }
  }
}