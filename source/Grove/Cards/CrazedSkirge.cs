namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;

  public class CrazedSkirge : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Crazed Skirge")
        .ManaCost("{3}{B}")
        .Type("Creature Imp")
        .Text("{Flying}, {haste}")
        .FlavorText("They are Phyrexia's couriers; the messages they carry are inscribed on their slick hides.")
        .Power(2)
        .Toughness(2)
        .Cast(p => p.Timing = Timings.FirstMain())        
        .Abilities(
          Static.Flying,
          Static.Haste
        );
    }
  }
}