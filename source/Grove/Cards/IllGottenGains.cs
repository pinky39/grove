namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.CastingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Zones;

  public class IllGottenGains : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.TimingRule(new OnSecondMain());
            p.TimingRule(new WhenYourHandCountIs(minCount: 0, maxCount: 2));
          });
    }
  }
}