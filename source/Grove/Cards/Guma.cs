namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Mana;

  public class Guma : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Guma")
        .ManaCost("{2}{R}")
        .Type("Creature Cat")
        .Text("{Protection from blue}")
        .FlavorText(
          "When the giant returned for the night, he found a dead merfolk on his pillow. Although he praised the little guma, he inwardly wondered where she had hid the head.")
        .Power(2)
        .Toughness(2)
        .Protections(ManaColors.Blue);
    }
  }
}