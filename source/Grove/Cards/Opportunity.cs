namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Opportunity : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Opportunity")
        .ManaCost("{4}{U}{U}")
        .Type("Instant")
        .Text("Target player draws four cards.")
        .FlavorText(
          "He cocooned himself alone in his workshop for months. When he finally emerged, all broad grins and excited chatter, I knew he'd found his answer.")
        .Cast(p =>
          {
            p.Effect = () => new TargetPlayerDrawsCards(4);
            p.TargetSelector.AddEffect(trg => trg.Is.Player());

            p.TimingRule(new EndOfTurn());
            p.TargetingRule(new SpellOwner());
          });
    }
  }
}