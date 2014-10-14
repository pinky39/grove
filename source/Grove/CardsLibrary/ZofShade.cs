namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class ZofShade : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Zof Shade")
        .ManaCost("{3}{B}")
        .Type("Creature — Shade")
        .Text("{2}{B}: Zof Shade gets +2/+2 until end of turn.")
        .FlavorText("Shades are drawn to places of power, often rooting themselves in a single area to feed.")
        .Power(2)
        .Toughness(2)
        .Pump(
          cost: "{2}{B}".Parse(),
          text: "{2}{B}: Zof Shade gets +2/+2 until end of turn.",
          powerIncrease: 2,
          toughnessIncrease: 2);
    }
  }
}