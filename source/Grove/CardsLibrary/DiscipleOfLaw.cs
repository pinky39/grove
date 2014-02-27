namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class DiscipleOfLaw : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Disciple of Law")
        .ManaCost("{1}{W}")
        .Type("Creature - Human Cleric")
        .Text("Protection from red{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .FlavorText("A religious order for religious order.")
        .Power(1)
        .Toughness(2)
        .Protections(CardColor.Red)
        .Cycling("{2}");
    }
  }
}