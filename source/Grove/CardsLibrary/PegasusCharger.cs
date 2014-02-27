namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class PegasusCharger : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Pegasus Charger")
        .ManaCost("{2}{W}")
        .Type("Creature Pegasus")
        .Text("{Flying, first strike}")
        .FlavorText(
          "The clouds came alive and dove to the earth Hooves flashed among the dark army, who fled before the spectacle of fury.")
        .Power(2)
        .Toughness(1)
        .SimpleAbilities(
          Static.Flying,
          Static.FirstStrike
        );
    }
  }
}