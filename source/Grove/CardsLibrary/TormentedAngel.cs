namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class TormentedAngel : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Tormented Angel")
        .ManaCost("{3}{W}")
        .Type("Creature - Angel")
        .Text("{Flying}")
        .FlavorText(
          "Falling from heaven is not as painful as surviving the impact.")
        .Power(1)
        .Toughness(5)
        .SimpleAbilities(Static.Flying);
    }
  }
}