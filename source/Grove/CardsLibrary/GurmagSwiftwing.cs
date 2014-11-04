namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class GurmagSwiftwing : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Gurmag Swiftwing")
        .ManaCost("{1}{B}")
        .Type("Creature — Bat")
        .Text("{Flying}, {first strike}, {haste}")
        .FlavorText("\"Anything a falcon can do, a bat can do in pitch darkness.\"{EOL}—Urdnan the Wanderer")
        .Power(1)
        .Toughness(2)
        .SimpleAbilities(Static.Flying, Static.FirstStrike, Static.Haste);
    }
  }
}
