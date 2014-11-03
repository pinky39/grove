namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class AlabasterKirin : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Alabaster Kirin")
        .ManaCost("{3}{W}")
        .Type("Creature — Kirin")
        .Text("{Flying}, {vigilance}")
        .FlavorText("The appearance of a kirin signifies the passing or arrival of an important figure. As word of sightings spread, all the khans took it to mean themselves. Only the shaman Chianul thought of Sarkhan Vol.")
        .Power(2)
        .Toughness(3)
        .SimpleAbilities(Static.Flying, Static.Vigilance);
    }
  }
}
