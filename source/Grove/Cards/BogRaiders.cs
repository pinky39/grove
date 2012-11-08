namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Dsl;

  public class BogRaiders : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Bog Raiders")
        .ManaCost("{2}{B}")
        .Type("Creature Zombie")
        .Text("{Swampwalk} (This creature is unblockable as long as defending player controls a Swamp.)")
        .FlavorText(
          "'Let weak feed on weak, that we may divine the nature of strength.'{EOL}—Phyrexian Scriptures")
        .Power(2)
        .Toughness(2)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Swampwalk
        );
    }
  }
}