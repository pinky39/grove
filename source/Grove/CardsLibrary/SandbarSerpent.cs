namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class SandbarSerpent : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sandbar Serpent")
        .ManaCost("{4}{U}")
        .Type("Creature - Serpent")
        .Text("{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .FlavorText(
          "Treacherous and unpredictable currents around Tolaria earned the nickname 'serpent wakes.'")
        .Power(3)
        .Toughness(4)
        .Cycling("{2}");
    }
  }
}