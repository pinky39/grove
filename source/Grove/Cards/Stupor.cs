namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;

  public class Stupor : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.TimingRule(new FirstMain());
          });
    }
  }
}