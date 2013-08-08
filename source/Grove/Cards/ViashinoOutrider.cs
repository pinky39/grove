namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Gameplay.Misc;

  public class ViashinoOutrider : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Viashino Outrider")
        .ManaCost("{2}{R}")
        .Type("Creature Viashino")
        .Text(
          "{Echo} {2}{R} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.)")
        .FlavorText("Give thy ration to thy mount, if the road be long. So sayeth the bey.")
        .Power(4)
        .Toughness(3)
        .Echo("{2}{R}");
    }
  }
}