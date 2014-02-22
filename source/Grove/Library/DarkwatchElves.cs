namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class DarkwatchElves : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Darkwatch Elves")
        .ManaCost("{2}{G}")
        .Type("Creature Elf")
        .Text("{Protection from black}{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Power(2)
        .Toughness(2)
        .Cycling("{2}")
        .Protections(CardColor.Black);
    }
  }
}