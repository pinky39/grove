namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Effects;

  public class Gamble : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
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
            p.Timing = Timings.FirstMain();
            p.Effect = Effect<SearchLibraryPutToHand>(e =>
              {
                e.MinCount = 1;
                e.MaxCount = 1;
                e.DiscardRandomCardAfterwards = true;
                e.RevealCards = false;
                e.Text = "Select a card from your library.";
              });
          });
    }
  }
}