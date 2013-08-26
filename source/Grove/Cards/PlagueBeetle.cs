namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Gameplay.Abilities;
  using Gameplay.Misc;

  public class PlagueBeetle : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Plague Beetle")
        .ManaCost("{B}")
        .Type("Creature Insect")
        .Text("{Swampwalk}")
        .FlavorText("It is the harbinger of disease, not the carrier.")
        .Power(1)
        .Toughness(1)
        .SimpleAbilities(Static.Swampwalk);
    }
  }
}