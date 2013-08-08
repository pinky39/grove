namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class TolarianWinds : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Tolarian Winds")
        .ManaCost("{1}{U}")
        .Type("Instant")
        .Text("Discard all the cards in your hand, then draw that many cards.")
        .FlavorText("Afterward, Tolaria's winds were like the whispers of lost wizards, calling for life.")
        .Cast(p =>
          {
            p.Effect = () => new DiscardAndDrawANewHand();
            p.TimingRule(new EndOfTurn());
            p.TimingRule(new ControllerHandCountIs(minCount: 2));
          });
    }
  }
}