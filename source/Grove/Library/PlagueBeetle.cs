namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

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