namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class MonasterySwiftspear : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Monastery Swiftspear")
        .ManaCost("{R}")
        .Type("Creature — Human Monk")
        .Text("{Haste}{EOL}{Prowess} {I}(Whenever you cast a noncreature spell, this creature gets +1/+1 until end of turn.){/I}")
        .FlavorText("The calligraphy of combat is written with strokes of sudden blood.")
        .Power(1)
        .Toughness(2)
        .Prowess()
        .SimpleAbilities(Static.Haste);
    }
  }
}
