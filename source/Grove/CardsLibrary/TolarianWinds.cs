namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;

  public class TolarianWinds : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Tolarian Winds")
        .ManaCost("{1}{U}")
        .Type("Instant")
        .Text("Discard all the cards in your hand, then draw that many cards.")
        .FlavorText("Afterward, Tolaria's winds were like the whispers of lost wizards, calling for life.")
        .Cast(p =>
          {
            p.Effect = () => new DiscardAndDrawANewHand();
            p.TimingRule(new OnEndOfOpponentsTurn());
            p.TimingRule(new WhenYourHandCountIs(minCount: 2));
          });
    }
  }
}