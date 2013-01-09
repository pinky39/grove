namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Dsl;

  public class Evacuation : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Evacuation")
        .ManaCost("{3}{U}{U}")
        .Type("Instant")
        .Text("Return all creatures to their owners' hands.")
        .FlavorText("The first step of every exodus is from the blood and the fire onto the trail.")
        .Cast(p =>
          {
            p.Category = EffectCategories.Bounce;
            p.Timing = Timings.InstantBounceAllCreatures();
            p.Effect = Effect<ReturnAllPermanentsToHand>(e => e.Filter = (permanent) => permanent.Is().Creature);
          });
    }
  }
}