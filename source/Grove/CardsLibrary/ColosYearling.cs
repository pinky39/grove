namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class ColosYearling : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Colos Yearling")
        .ManaCost("{2}{R}")
        .Type("Creature Goat Beast")
        .Text("{Mountainwalk}{EOL}{R}: Colos Yearling gets +1/+0 until end of turn.")
        .FlavorText("A steed grows with its rider.")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.Mountainwalk)
        .Pump(
          cost: Mana.Red,
          text: "{R}: Colos Yearling gets +1/+0 until end of turn.",
          powerIncrease: 1,
          toughnessIncrease: 0);
    }
  }
}