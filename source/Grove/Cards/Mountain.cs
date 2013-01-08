namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Mana;

  public class Mountain : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Mountain")
        .Type("Basic Land - Mountain")
        .Text("{T}: Add {R} to your mana pool.")
        .Abilities(
          ManaAbility(ManaUnit.Red, "{T}: Add {R} to your mana pool."));
    }
  }
}