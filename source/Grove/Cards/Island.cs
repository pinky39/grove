namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Card.Factory;
  using Gameplay.Mana;

  public class Island : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Island")
        .Type("Basic Land - Island")
        .Text("{T}: Add {U} to your mana pool.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {U} to your mana pool.";
            p.ManaAmount(Mana.Blue);
          });
    }
  }
}