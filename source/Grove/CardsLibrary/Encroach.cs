namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class Encroach : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Encroach")
        .ManaCost("{B}")
        .Type("Sorcery")
        .Text(
          "Target player reveals his or her hand. You choose a nonbasic land card from it. That player discards that card.")
        .FlavorText("The rate of spread in this region is unacceptable. Start again.")
        .Cast(p =>
          {
            p.Effect = () => new OpponentDiscardsCards(
              selectedCount: 1,
              youChooseDiscardedCards: true,
              filter: card => card.Is().NonBasicLand);

            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenOpponentsHandCountIs(minCount: 2));
          });
    }
  }
}