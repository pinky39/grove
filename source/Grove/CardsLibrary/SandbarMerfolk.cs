namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class SandbarMerfolk : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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