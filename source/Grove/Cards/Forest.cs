namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;

  public class Forest : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Forest")
        .Type("Basic Land - Forest")
        .Text("{T}: Add {G} to your mana pool.")
        .Timing(Timings.Lands)
        .Abilities(
          C.ManaAbility(Mana.Green, "{T}: Add {G} to your mana pool."));
    }
  }
}