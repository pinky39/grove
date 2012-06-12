namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;

  public class Island : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Island")
        .Type("Basic Land - Island")
        .Text("{T}: Add {U} to your mana pool.")
        .Timing(Timings.Lands)
        .Abilities(
          C.ManaAbility(Mana.Blue, "{T}: Add {U} to your mana pool."));
    }
  }
}