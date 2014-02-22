namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class BloatedToad : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bloated Toad")
        .ManaCost("{2}{G}")
        .Type("Creature Frog")
        .Text("{Protection from blue}{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")        
        .Power(2)
        .Toughness(2)
        .Cycling("{2}")
        .Protections(CardColor.Blue);
    }
  }
}