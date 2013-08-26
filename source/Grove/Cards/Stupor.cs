namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Stupor : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Stupor")
        .ManaCost("{2}{B}")
        .Type("Sorcery")
        .Text("Target opponent discards a card at random, then discards a card.")
        .FlavorText("There are medicines for all afflictions but idleness.")
        .Cast(p =>
          {
            p.Effect = () => new OpponentDiscardsCards(randomCount: 1, selectedCount: 1);
            p.TimingRule(new OnFirstMain());
          });
    }
  }
}