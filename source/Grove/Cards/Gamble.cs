namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;

  public class Gamble : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Gamble")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text(
          "Search your library for a card, put that card into your hand, discard a card at random, then shuffle your library.")
        .FlavorText("When you've got nothing, you might as well trade it for something else.");

    }
  }
}