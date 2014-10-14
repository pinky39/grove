namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class FurnaceWhelp : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Furnace Whelp")
        .ManaCost("{2}{R}{R}")
        .Type("Creature - Dragon")
        .Text("{Flying}{EOL}{R}: Furnace Whelp gets +1/+0 until end of turn.")
        .FlavorText("Baby dragons can't figure out humans—if they didn't want to be killed, why were they made of meat and treasure?")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(Static.Flying)
        .Pump(
          cost: Mana.Red,
          text: "{R}: Furnace Whelp gets +1/+0 until end of turn.",
          powerIncrease: 1,
          toughnessIncrease: 0);
    }
  }
}
