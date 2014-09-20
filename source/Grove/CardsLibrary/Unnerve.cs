namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Grove.Effects;
  using Grove.AI.TimingRules;

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