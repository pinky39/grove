namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class CapashenKnight : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Capashen Knight")
        .ManaCost("{1}{W}")
        .Type("Creature Human Knight")
        .Text("{First strike}{EOL}{1}{W}: Capashen Knight gets +1/+0 until end of turn.")
        .FlavorText("Few warriors dare to challenge a knight of Capashen. Should one do so, there is one fewer.")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.FirstStrike)
        .Pump(
          cost: "{1}{W}".Parse(),
          text: "{1}{W}: Capashen Knight gets +1/+0 until end of turn.",
          powerIncrease: 1,
          toughnessIncrease: 0);
    }
  }
}