namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class GrizzlyBears : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Grizzly Bears")
        .ManaCost("{1}{G}")
        .Type("Creature - Bear")
        .FlavorText(
          "We cannot forget that among all of Dominaria's wonders, a system of life exists, with prey and predators that will never fight wars nor vie for ancient power.")
        .Power(2)
        .Toughness(2);
    }
  }
}