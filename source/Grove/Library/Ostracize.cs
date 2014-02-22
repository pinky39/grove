namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;

  public class Ostracize : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Ostracize")
        .ManaCost("{B}")
        .Type("Sorcery")
        .Text(
          "Target opponent reveals his or her hand. You choose a creature card from it. That player discards that card.")
        .FlavorText("Kerrick was to find that some borders can never be crossed.")
        .Cast(p =>
          {
            p.Effect = () => new OpponentDiscardsCards(
              selectedCount: 1,
              youChooseDiscardedCards: true,
              filter: card => card.Is().Creature);

            p.TimingRule(new OnFirstMain());
            p.TimingRule(new WhenOpponentsHandCountIs(minCount: 2));
          });
    }
  }
}