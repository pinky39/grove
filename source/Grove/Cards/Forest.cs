namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Mana;

  public class Forest : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Forest")
        .Type("Basic Land - Forest")
        .Text("{T}: Add {G} to your mana pool.")
        .Abilities(
          ManaAbility(ManaUnit.Green, "{T}: Add {G} to your mana pool."));
    }
  }
}