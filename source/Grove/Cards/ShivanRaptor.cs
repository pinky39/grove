namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Dsl;


  public class ShivanRaptor : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Shivan Raptor")
        .ManaCost("{2}{R}")
        .Type("Creature Lizard")
        .Text(
          "{First strike}, {haste}{EOL}Echo {2}{R} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.)")
        .Power(3)
        .Toughness(1)
        .Timing(Timings.FirstMain())
        .Echo("{2}{R}")
        .Abilities(
          Static.Haste,
          Static.FirstStrike
        );
    }
  }
}