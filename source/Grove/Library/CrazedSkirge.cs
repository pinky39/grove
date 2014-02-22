namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;

  public class CrazedSkirge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Crazed Skirge")
        .ManaCost("{3}{B}")
        .Type("Creature Imp")
        .Text("{Flying}, {haste}")
        .FlavorText("They are Phyrexia's couriers; the messages they carry are inscribed on their slick hides.")
        .Power(2)
        .Toughness(2)
        .SimpleAbilities(
          Static.Flying,
          Static.Haste
        );
    }
  }
}