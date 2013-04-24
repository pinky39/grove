﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;

  public class GoblinSpelunkers : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Goblin Spelunkers")
        .ManaCost("{2}{R}")
        .Type("Creature Goblin Warrior")
        .Text("{Mountainwalk}")
        .FlavorText("'It only short jump. You go first.'{EOL}'AIIIEEEE'{EOL}'Hmm . . . we go different way now.'")
        .Power(2)
        .Toughness(2)
        .StaticAbilities(Static.Mountainwalk);
    }
  }
}