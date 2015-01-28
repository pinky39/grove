namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class LotusPathDjinn : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Lotus Path Djinn")
        .ManaCost("{3}{U}")
        .Type("Creature — Djinn Monk")
        .Text("{Flying}{EOL}{Prowess} {I}(Whenever you cast a noncreature spell, this creature gets +1/+1 until end of turn.){/I}")
        .FlavorText("\"The lotus takes root where body and mind intersect. It blooms when body and mind become one.\"")
        .Power(2)
        .Toughness(3)
        .Prowess()
        .SimpleAbilities(Static.Flying);
    }
  }
}
