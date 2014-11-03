namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class HighspireMantis : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Highspire Mantis")
        .ManaCost("{2}{R}{W}")
        .Type("Creature — Insect")
        .Text("{Flying}, {trample}")
        .FlavorText("ts wings produce a high-pitched, barely audible whirring sound in flight. Only Jeskai masters are quiet enough to hear one coming.")
        .Power(3)
        .Toughness(3)
        .SimpleAbilities(Static.Flying, Static.Trample);
    }
  }
}
