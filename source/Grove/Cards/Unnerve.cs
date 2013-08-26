namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;

  public class Unnerve : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Unnerve")
        .ManaCost("{3}{B}")
        .Type("Sorcery")
        .Text("Each opponent discards two cards.")
        .FlavorText("If fear is the only tool you have left, then you'll never control me.")
        .Cast(p =>
          {
            p.Effect = () => new OpponentDiscardsCards(selectedCount: 2);
            p.TimingRule(new OnFirstMain());
          });
    }
  }
}