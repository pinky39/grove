namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.CastingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Zones;

  public class IllGottenGains : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Ill-Gotten Gains")
        .ManaCost("{2}{B}{B}")
        .Type("Sorcery")
        .Text(
          "Exile Ill-Gotten Gains. Each player discards his or her hand, then returns up to three cards from his or her graveyard to his or her hand.")
        .FlavorText("Urza thought it a crusade. Xantcha knew it was a robbery.")
        .Cast(p =>
          {
            p.Effect = () => new CompoundEffect(
              new EachPlayerDiscardsHand(),
              new EachPlayerReturnsCardsToHand(
                minCount: 0,
                maxCount: 3,
                zone: Zone.Graveyard,
                aiOrdersByDescendingScore: false,
                text: "Select cards in your graveyard to return to hand."));

            p.Rule = new Sorcery(afterResolvePutToZone: c => c.Exile());
            p.TimingRule(new SecondMain());
            p.TimingRule(new ControllerHandCountIs(minCount: 0, maxCount: 2));
          });
    }
  }
}