namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Casting;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Zones;

  public class IllGottenGains : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
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
            p.Timing = All(Timings.SecondMain(), Timings.HasLessThanCardsInHand(count: 2));
            p.Effect = Effect<CompoundEffect>(e => e.ChildEffects(
              Effect<EachPlayerDiscardsHand>(),
              Effect<EachPlayerReturnsCardsToHand>(e1 =>
                {                      
                  e1.MinCount = 0;
                  e1.MaxCount = 3;
                  e1.Zone = Zone.Graveyard;
                  e1.AiOrdersByDescendingScore = false;
                  e1.Text = "Select cards to return to hand";
                })));
            p.Rule = Rule<Sorcery>(r => r.AfterResolvePutToZone = c => c.Exile());
          });
    }
  }
}