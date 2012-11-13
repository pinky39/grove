namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Mana;

  public class Plains : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Plains")
        .Type("Basic Land - Plains")
        .Text("{T}: Add {W} to your mana pool.")
        .Timing(Timings.Lands())
        .Abilities(
          ManaAbility(ManaUnit.White, "{T}: Add {W} to your mana pool."));
    }
  }
}