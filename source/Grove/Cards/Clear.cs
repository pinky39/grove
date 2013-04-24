﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Player;

  public class Clear : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Clear")
        .ManaCost("{1}{W}")
        .Type("Instant")
        .Text("Destroy target enchantment.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Enchantment().On.Battlefield());
            
            p.TargetingRule(new OrderByRank(c => -c.Score, ControlledBy.Opponent));
            p.TimingRule(new TargetRemoval());
          });
    }
  }
}