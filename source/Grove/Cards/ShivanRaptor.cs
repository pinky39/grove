namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class ShivanRaptor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Shivan Raptor")
        .ManaCost("{2}{R}")
        .Type("Creature Lizard")
        .Text(
          "{First strike}, {haste}{EOL}Echo {2}{R} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.)")
        .Power(3)
        .Toughness(1)
        .Echo("{2}{R}")
        .SimpleAbilities(
          Static.Haste,
          Static.FirstStrike
        );
    }
  }
}