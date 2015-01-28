namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class DragonStyleTwins : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Dragon-Style Twins")
        .ManaCost("{3}{R}{R}")
        .Type("Creature — Human Monk")
        .Text("{Double strike}{EOL}{Prowess} {I}(Whenever you cast a noncreature spell, this creature gets +1/+1 until end of turn.){/I}")
        .FlavorText("\"We are the flicker of the flame and its heat, the two sides of a single blade.\"")
        .Power(3)
        .Toughness(3)
        .Prowess()
        .SimpleAbilities(Static.DoubleStrike);
    }
  }
}
