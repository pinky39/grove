﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Mana;

  public class Forest : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Forest")
        .Type("Basic Land - Forest")
        .Text("{T}: Add {G} to your mana pool.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {G} to your mana pool.";
            p.ManaAmount(Mana.Green);
          });
    }
  }
}