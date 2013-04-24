namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Catalog : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.TimingRule(new EndOfTurn());
          });
    }
  }
}