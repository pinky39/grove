namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class BlastedLandscape : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Blasted Landscape")
        .Type("Land")
        .Text("{T}: Add one colorless mana to your mana pool.{EOL}{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add one colorless mana to your mana pool.";
            p.ManaAmount(1.Colorless());
          });
    }
  }
}