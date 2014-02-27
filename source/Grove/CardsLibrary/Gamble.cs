namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;

  public class Gamble : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
              () => new SearchLibraryPutToZone(
                zone: Zone.Hand,
                minCount: 1,
                maxCount: 1,
                revealCards: false)
                {AfterResolve = e => e.Controller.DiscardRandomCard()};

            p.TimingRule(new OnFirstMain());
          });
    }
  }
}