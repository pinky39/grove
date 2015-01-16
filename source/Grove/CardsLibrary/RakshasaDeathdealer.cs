namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class RakshasaDeathdealer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rakshasa Deathdealer")
        .ManaCost("{B}{G}")
        .Type("Creature - Demon Cat")
        .Text("{B}{G}: Rakshasa Deathdealer gets +2/+2 until end of turn.{EOL}{B}{G}: Regenerate Rakshasa Deathdealer.")
        .FlavorText("\"Death fills me and makes me strong. You, it will reduce to nothing.\"")
        .Power(2)
        .Toughness(2)
        .Pump(
          cost: "{B}{G}".Parse(),
          text: "{B}{G}: Rakshasa Deathdealer gets +2/+2 until end of turn.",
          powerIncrease: 2,
          toughnessIncrease: 2)
        .Regenerate(cost: "{B}{G}".Parse(), text: "{B}{G}: Regenerate Rakshasa Deathdealer.");
    }
  }
}
