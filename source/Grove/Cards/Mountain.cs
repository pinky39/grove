namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Mana;
  using Core.Dsl;

  public class Mountain : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Mountain")
        .Type("Basic Land - Mountain")
        .Text("{T}: Add {R} to your mana pool.")
        .Timing(Timings.Lands())
        .Abilities(
          C.ManaAbility(ManaUnit.Red, "{T}: Add {R} to your mana pool."));
    }
  }
}