namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TimingRules;
  using Effects;

  public class DiplomacyOfTheWastes : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Diplomacy of the Wastes")
        .ManaCost("{2}{B}")
        .Type("Sorcery")
        .Text("Target opponent reveals his or her hand. You choose a nonland card from it. That player discards that card. If you control a Warrior, that player loses 2 life.")
        .FlavorText("\"Our emissaries are gifted negotiators.\"{EOL}—Alesha, Who Smiles at Death")
        .Cast(p =>
        {
          p.Effect = () => new CompoundEffect(
            new OpponentDiscardsCards(
              selectedCount: 1,
              youChooseDiscardedCards: true,
              filter: card => !card.Is().Land),
            new ChangeLife(P(e => e.Controller.Battlefield.Creatures.Any(x => x.Is("warrior")) ? -2 : 0), opponents: true));

          p.TimingRule(new OnFirstMain());
          p.TimingRule(new WhenOpponentsHandCountIs(minCount: 2));
        });
    }
  }
}
