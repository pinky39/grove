namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Zones;

  public class Curfew : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Curfew")
        .ManaCost("{U}")
        .Type("Instant")
        .Text("Each player returns a creature he or she controls to its owner's hand.")
        .FlavorText(". . . But I'm not tired'")
        .Cast(p =>
          {
            p.Timing = Timings.InstantRemovalPlayerChooses(1);
            p.Effect = Effect<EachPlayerReturnsCardsToHand>(e =>
              {
                e.Filter = c => c.Is().Creature;
                e.MinCount = 1;
                e.MaxCount = 1;
                e.Zone = Zone.Battlefield;
                e.AiOrdersByDescendingScore = false;
                e.Text = "Select creature to return to hand";
              });
          });
    }
  }
}