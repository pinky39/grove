namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Mana;
  using Core.Dsl;

  public class Swamp : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Swamp")
        .Type("Basic Land - Swamp")
        .Text("{T}: Add {B} to your mana pool.")
        .Timing(Timings.Lands())
        .Abilities(
          C.ManaAbility(ManaUnit.Black, "{T}: Add {B} to your mana pool."));
    }
  }
}