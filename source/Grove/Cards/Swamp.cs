namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Dsl;
  using Core.Mana;

  public class Swamp : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Swamp")
        .Type("Basic Land - Swamp")
        .Text("{T}: Add {B} to your mana pool.")
        .Abilities(
          ManaAbility(ManaUnit.Black, "{T}: Add {B} to your mana pool."));
    }
  }
}