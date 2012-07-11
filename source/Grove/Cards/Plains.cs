namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Mana;
  using Core.Dsl;

  public class Plains : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Plains")
        .Type("Basic Land - Plains")
        .Text("{T}: Add {W} to your mana pool.")
        .Timing(Timings.Lands())
        .Abilities(
          C.ManaAbility(ManaUnit.White, "{T}: Add {W} to your mana pool."));
    }
  }
}