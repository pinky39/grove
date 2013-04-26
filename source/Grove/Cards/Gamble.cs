namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Gamble : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Gamble")
        .ManaCost("{R}")
        .Type("Sorcery")
        .Text(
          "Search your library for a card, put that card into your hand, discard a card at random, then shuffle your library.")
        .FlavorText("When you've got nothing, you might as well trade it for something else.")
        .Cast(p =>
          {
            p.Effect =
              () => new SearchLibraryPutToZone(c => c.PutToHand(), minCount: 1, maxCount: 1, revealCards: false)
                {AfterResolve = e => e.Controller.DiscardRandomCard()};

            p.TimingRule(new FirstMain());
          });
    }
  }
}