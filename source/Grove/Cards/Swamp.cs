namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;

  public class Swamp : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Swamp")
        .Type("Basic Land - Swamp")
        .Text("{T}: Add {B} to your mana pool.")
        .Timing(Timings.Lands)
        .Abilities(
          C.ManaAbility(Mana.Black, "{T}: Add {B} to your mana pool."));
    }
  }
}