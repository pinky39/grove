namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Mana;

  public class Island : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Island")
        .Type("Basic Land - Island")
        .Text("{T}: Add {U} to your mana pool.")
        .Abilities(
          ManaAbility(ManaUnit.Blue, "{T}: Add {U} to your mana pool."));
    }
  }
}