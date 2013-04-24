﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Characteristics;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Hibernation : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Hibernation")
        .ManaCost("{2}{U}")
        .Type("Instant")
        .Text("Return all green permanents to their owners' hands.")
        .FlavorText(
          "On its way to the cave, the armadillo brushed by a sapling. It awoke to find a full-grown tree blocking its path.")
        .Cast(p =>
          {
            p.Effect = () => new ReturnAllPermanentsToHand((c) => c.HasColor(CardColor.Green));
            p.TimingRule(new BounceAll());
          });
    }
  }
}