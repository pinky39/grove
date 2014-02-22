namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class WildColos : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Wild Colos")
        .ManaCost("{2}{R}")
        .Type("Creature Goat Beast")
        .Text("{Haste}")
        .FlavorText("You'll never get a Keldon's goat.")
        .Power(2)
        .Toughness(2)                
        .SimpleAbilities(Static.Haste);
    }
  }
}