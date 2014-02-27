namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class Acridian : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Acridian")
        .ManaCost("{1}{G}")
        .Type("Creature Insect")
        .Text(
          "{Echo} {1}{G}")
        .FlavorText(
          "The elves of Argoth were trained to ride these creatures, even when their mounts traveled upside-down.")
        .Power(2)
        .Toughness(4)
        .Echo("{1}{G}");
    }
  }
}