namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;

  public class Catalog : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Catalog")
        .ManaCost("{2}{U}")
        .Type("Instant")
        .Text("Draw two cards, then discard a card.")
        .FlavorText("'Without order comes errors, and errors kill on Tolaria.'{EOL}—Barrin, master wizard")
        .Cast(p =>
          {
            p.Effect = () => new DrawCards(2, discardCount: 1);
            p.TimingRule(new EndOfTurn());
          });
    }
  }
}