namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class ThunderingGiant : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thundering Giant")
        .ManaCost("{3}{R}{R}")
        .Type("Creature Giant")
        .Text("{Haste}")
        .FlavorText("The giant was felt a few seconds before he was seen.")
        .Power(4)
        .Toughness(3)
        .SimpleAbilities(Static.Haste);
    }
  }
}