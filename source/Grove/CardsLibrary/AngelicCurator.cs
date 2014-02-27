namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class AngelicCurator : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Angelic Curator")
        .ManaCost("{1}{W}")
        .Type("Creature Angel Spirit")
        .Text("{Flying}, protection from artifacts")
        .FlavorText(
          "Do not treat your people as you treat your artifacts. Let them go, and they will live; seal them here, and they will die.")
        .Power(1)
        .Toughness(1)
        .Protections("artifact")
        .SimpleAbilities(Static.Flying);
    }
  }
}