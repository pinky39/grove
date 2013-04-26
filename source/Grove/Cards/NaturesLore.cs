namespace Grove.Cards
{
  using System.Collections.Generic;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class NaturesLore : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Nature's Lore")
        .ManaCost("{1}{G}")
        .Type("Sorcery")
        .Text(
          "Search your library for a Forest card and put that card onto the battlefield. Then shuffle your library.")
        .FlavorText("Every seed planted is another lesson the forest can teach us.")
        .Cast(p =>
          {
            p.Effect = () => new SearchLibraryPutToZone(
              c => c.PutToBattlefield(),
              minCount: 0,
              maxCount: 1,
              validator: (e, c) => c.Is("forest"),
              text: "Search your library for a Forest card.");
          });
    }
  }
}