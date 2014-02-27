namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;

  public class Catalog : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Catalog")
        .ManaCost("{2}{U}")
        .Type("Instant")
        .Text("Draw two cards, then discard a card.")
        .FlavorText("Without order comes errors, and errors kill on Tolaria.")
        .Cast(p =>
          {
            p.Effect = () => new DrawCards(2, discardCount: 1);
            p.TimingRule(new OnEndOfOpponentsTurn());
          });
    }
  }
}