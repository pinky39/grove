namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class ScentOfJasmine : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scent of Jasmine")
        .ManaCost("{W}")
        .Type("Instant")
        .Text("Reveal any number of white cards in your hand. You gain 2 life for each card revealed this way.")
        .Cast(p =>
          {
            p.Effect = () => new GainLifeForEachRevealedCard(c => c.HasColor(CardColor.White), 2);
            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.HasColor(CardColor.White)));
            p.TimingRule(new OnEndOfOpponentsTurn());
          });
    }
  }
}