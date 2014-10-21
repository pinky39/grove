namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class WallOfFire : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Wall of Fire")
        .ManaCost("{1}{R}{R}")
        .Type("Creature — Wall")
        .Text("{Defender}{I}(This creature can't attack.){/I}{EOL}{R}: Wall of Fire gets +1/+0 until end of turn.")
        .FlavorText("Mercy is for those who keep their distance.")
        .Power(0)
        .Toughness(5)
        .SimpleAbilities(Static.Defender)
        .Pump(
          cost: "{R}".Parse(),
          text: "{R}: Wall of Fire gets +1/+0 until end of turn.",
          powerIncrease: 1,
          toughnessIncrease: 0);
    }
  }
}