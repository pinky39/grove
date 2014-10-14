namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class MahamotiDjinn : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Mahamoti Djinn")
        .ManaCost("{4}{U}{U}")
        .Type("Creature - Djinn")
        .Text("{Flying} {I}(This creature can't be blocked except by creatures with flying or reach.){/I}")
        .FlavorText("Of royal blood among the spirits of the air, the Mahamoti djinn rides on the wings of the winds. As dangerous in the gambling hall as he is in battle, he is a master of trickery and misdirection.")
        .Power(5)
        .Toughness(6)
        .SimpleAbilities(Static.Flying);
    }
  }
}
