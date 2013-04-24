namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Mana;

  public class Plains : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Plains")
        .Type("Basic Land - Plains")
        .Text("{T}: Add {W} to your mana pool.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {W} to your mana pool.";
            p.ManaAmount(Mana.White);
          });
    }
  }
}