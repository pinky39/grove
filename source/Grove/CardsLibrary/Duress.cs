namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;
    
  public class Duress : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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

            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenOpponentsHandCountIs(minCount: 2));
          });
    }
  }
}