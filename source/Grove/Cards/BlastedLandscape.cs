namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Mana;
  using Core.Dsl;

  public class BlastedLandscape : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Blasted Landscape")
        .Type("Land")
        .Text("{T}: Add one colorless mana to your mana pool.{EOL}{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .Timing(Timings.Lands())
        .Cycling("{2}")
        .Abilities(
          C.ManaAbility(new ManaUnit(ManaColors.Colorless), "{T}: Add one colorless mana to your mana pool."));
    }
  }
}