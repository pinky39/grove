namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class NantukoShade : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Nantuko Shade")
        .ManaCost("{B}{B}")
        .Type("Creature - Insect Shade")
        .Text("{B}: Nantuko Shade gets +1/+1 until end of turn.")
        .FlavorText("In life, the nantuko study nature by revering it. In death, they study nature by disemboweling it.")
        .Power(2)
        .Toughness(1)
        .Pump(
          cost: Mana.Black,
          text: "{B}: Nantuko Shade gets +1/+1 until end of turn.",
          powerIncrease: 1,
          toughnessIncrease: 1);
    }
  }
}