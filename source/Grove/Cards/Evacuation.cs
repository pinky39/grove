namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;

  public class Evacuation : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Evacuation")
        .ManaCost("{3}{U}{U}")
        .Type("Instant")
        .Text("Return all creatures to their owners' hands.")
        .FlavorText("The first step of every exodus is from the blood and the fire onto the trail.")
        .Category(EffectCategories.Bounce)
        .Timing(Timings.InstantBounceAllCreatures())
        .Effect<ReturnAllPermanentsToHand>((e, _) => e.Filter = (permanent) => permanent.Is().Creature);
    }
  }
}