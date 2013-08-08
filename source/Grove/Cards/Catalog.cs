namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

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
            p.TimingRule(new EndOfTurn());
          });
    }
  }
}