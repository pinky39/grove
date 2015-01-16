namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class Thoughtseize : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thoughtseize")
        .ManaCost("{B}")
        .Type("Sorcery")
        .Text("Target player reveals his or her hand. You choose a nonland card from it. That player discards that card. You lose 2 life.")
        .FlavorText("\"Knowledge is such a burden. Release it. Release all your fears to me.\"{EOL}—Ashiok, Nightmare Weaver")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new OpponentDiscardsCards(
              selectedCount: 1,
              youChooseDiscardedCards: true,
              filter: card => !card.Is().Land),
            new ChangeLife(-2, yours: true)); 

          p.TimingRule(new OnFirstMain());
          p.TimingRule(new WhenOpponentsHandCountIs(minCount: 2));
        });
    }
  }
}
