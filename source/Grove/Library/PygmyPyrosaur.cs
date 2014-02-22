namespace Grove.Library
{
  using System.Collections.Generic;
  using Grove.Gameplay;

  public class PygmyPyrosaur : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Pygmy Pyrosaur")
        .ManaCost("{1}{R}")
        .Type("Creature Lizard")
        .Text("Pygmy Pyrosaur can't block.{EOL}{R}: Pygmy Pyrosaur gets +1/+0 until end of turn.")
        .FlavorText("Do not judge a lizard by its size.")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.CannotBlock)
        .Pump(
          cost: Mana.Red,
          text: "{R}: Pygmy Pyrosaur gets +1/+0 until end of turn.",
          powerIncrease: 1,
          toughnessIncrease: 0);
    }
  }
}