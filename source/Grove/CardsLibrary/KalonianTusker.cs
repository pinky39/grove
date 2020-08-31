namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class KalonianTusker : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Kalonian Tusker")
        .ManaCost("{G}{G}")
        .Type("Creature - Beast")
        .FlavorText(
          "'And all this time I thought we were tracking it.'—Juruk, Kalonian tracker")
        .Power(3)
        .Toughness(3);
    }
  }
}