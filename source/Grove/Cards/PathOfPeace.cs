﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TargetingRules;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Targeting;

  public class PathOfPeace : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Path of Peace")
        .ManaCost("{3}{W}")
        .Type("Sorcery")
        .Text("Destroy target creature. Its owner gains 4 life.")
        .FlavorText(
          "When the sword becomes a burden, let the warrior lay it aside that another with a truer heart might take it up.")
        .Cast(p =>
          {
            p.Effect = () => new DestroyTargetPermanents
              {
                AfterResolve = e => e.Target.Card().Owner.Life += 4
              };

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TimingRule(new FirstMain());
            p.TargetingRule(new Destroy());
          });
    }
  }
}