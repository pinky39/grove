namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;

  public class Duress : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Duress")
        .ManaCost("{B}")
        .Type("Sorcery")
        .Text(
          "Target opponent reveals his or her hand. You choose a noncreature, nonland card from it. That player discards that card.")
        .FlavorText("We decide who is worthy of our works.")
        .Cast(p =>
          {
            p.Effect = () => new OpponentDiscardsCards(
              selectedCount: 1,
              youChooseDiscardedCards: true,
              filter: card => !card.Is().Creature && !card.Is().Land);

            p.TimingRule(new FirstMain());
            p.TimingRule(new OpponentHasAtLeastCardsInHand(2));
          });
    }
  }
}