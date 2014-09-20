namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class LoomingShade : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Looming Shade")
        .ManaCost("{2}{B}")
        .Type("Creature - Shade")
        .Text("{B}: Looming Shade gets +1/+1 until end of turn.")
        .FlavorText(
          "The shade tracks victims by reverberations of the pipes, as a spider senses prey tangled in its trembling web.")
        .Power(1)
        .Toughness(1)
        .Pump(
          cost: Mana.Black, 
          text: "{B}: Looming Shade gets +1/+1 until end of turn.", 
          powerIncrease: 1,
          toughnessIncrease: 1);
    }
  }
}