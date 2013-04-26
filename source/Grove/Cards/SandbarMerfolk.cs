namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Card.Factory;

  public class SandbarMerfolk : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Sandbar Merfolk")
        .ManaCost("{U}")
        .Type("Creature - Merfolk")
        .Text("{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .FlavorText(
          "You are not prey until a predator knows of your existence.")
        .Power(1)
        .Toughness(1)
        .Cycling("{2}");
    }
  }
}