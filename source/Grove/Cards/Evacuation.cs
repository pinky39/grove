namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Evacuation : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Evacuation")
        .ManaCost("{3}{U}{U}")
        .Type("Instant")
        .Text("Return all creatures to their owners' hands.")
        .FlavorText("The first step of every exodus is from the blood and the fire onto the trail.")
        .Cast(p =>
          {
            p.Effect = () => new ReturnAllPermanentsToHand(c => c.Is().Creature);
            p.TimingRule(new BounceAll());
          });
    }
  }
}