namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.CostRules;
  using AI.TimingRules;
  using Effects;
  using Modifiers;

  public class ReturnToTheRanks : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Return to the Ranks")
        .ManaCost("{W}{W}").HasXInCost()
        .Type("Sorcery")
        .Text("{Convoke} {I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for or one mana of that creature's color.){/I}{EOL}Return X target creature cards with converted mana cost 2 or less from your graveyard to the battlefield. ")
        .Convoke()
        .Cast(p =>
        {
          p.Effect = () => new PutSelectedCardsToBattlefield(
              "Select X creature cards in your graveyard.",
              c => c.Is().Creature && c.ConvertedCost <= 2,
              Zone.Graveyard, 
              afterCardPutToBattlefield: null, 
              count: Value.PlusX);

          p.TimingRule(new WhenYourGraveyardCountIs(minCount: 1, selector: c => c.Is().Creature && c.ConvertedCost <= 2));
          p.CostRule(new XIsAvailableMana());
        });
    }
  }
}
