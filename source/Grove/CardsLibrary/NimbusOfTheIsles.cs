namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class NimbusOfTheIsles : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Nimbus of the Isles")
        .ManaCost("{4}{U}")
        .Type("Creature — Elemental")
        .Text("{Flying} {I}(This creature can't be blocked except by creatures with flying or reach.){/I}")
        .FlavorText("The people of the Sevick Isles have a unique understanding of the term \"ominous clouds.\"")
        .Power(3)
        .Toughness(3)
        .SimpleAbilities(Static.Flying);
    }
  }
}
