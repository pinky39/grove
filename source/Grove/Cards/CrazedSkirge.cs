﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;

  public class CrazedSkirge : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Crazed Skirge")
        .ManaCost("{3}{B}")
        .Type("Creature Imp")
        .Text("{Flying}, {haste}")
        .FlavorText("They are Phyrexia's couriers; the messages they carry are inscribed on their slick hides.")
        .Power(2)
        .Toughness(2)
        .Cast(p => p.TimingRule(new FirstMain()))
        .StaticAbilities(
          Static.Flying,
          Static.Haste
        );
    }
  }
}