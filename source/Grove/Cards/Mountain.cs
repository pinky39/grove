﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Mana;

  public class Mountain : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Mountain")
        .Type("Basic Land - Mountain")
        .Text("{T}: Add {R} to your mana pool.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {R} to your mana pool.";
            p.ManaAmount(Mana.Red);
          });
    }
  }
}