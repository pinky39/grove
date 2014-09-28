namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class BronzeSable : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bronze Sable")
        .ManaCost("{2}")
        .Type("Artifact Creature — Sable")
        .FlavorText(
          "The Champion stood alone between the horde of the Returned and the shrine to Karametra, cutting down scores among hundreds. She would have been overcome if not for the aid of the temple guardians whom Karametra awakened.{EOL}—The Theriad")
        .Power(2)
        .Toughness(1);
    }
  }
}