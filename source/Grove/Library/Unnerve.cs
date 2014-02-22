namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

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