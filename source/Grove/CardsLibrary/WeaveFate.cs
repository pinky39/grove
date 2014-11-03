namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Effects;

  public class WeaveFate : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Weave Fate")
        .ManaCost("{3}{U}")
        .Type("Instant")
        .Text("Draw two cards.")
        .FlavorText("Temur shamans speak of three destinies: the now, the echo of the past, and the unwritten. They find flickering paths among tangled possibilities.")
        .Cast(p =>
        {
          p.Effect = () => new DrawCards(2);
        });
    }
  }
}
