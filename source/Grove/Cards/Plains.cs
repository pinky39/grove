namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;

  public class Plains : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Plains")
        .Type("Basic Land - Plains")
        .Text("{T}: Add {W} to your mana pool.")
        .Timing(Timings.Lands)
        .Abilities(
          C.ManaAbility(Mana.White, "{T}: Add {W} to your mana pool."));
    }
  }
}