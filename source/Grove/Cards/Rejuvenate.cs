namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;

  public class Rejuvenate : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Rejuvenate")
        .ManaCost("{3}{G}")
        .Type("Sorcery")
        .Text("You gain 6 life.{EOL}Cycling {2}({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Cast(p =>
          {
            p.Effect = () => new ControllerGainsLife(6);
            p.TimingRule(new SecondMain());
          });
    }
  }
}