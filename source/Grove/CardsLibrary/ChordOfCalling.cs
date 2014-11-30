namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.CostRules;
  using AI.TimingRules;
  using Effects;

  public class ChordOfCalling : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Chord of Calling")
        .ManaCost("{G}{G}{G}").HasXInCost()
        .Type("Instant")
        .Text("{Convoke} {I}(Your creatures can help cast this spell. Each creature you tap while casting this spell pays for {1} or one mana of that creature's color.){/I}{EOL}Search your library for a creature card with converted mana cost X or less and put it onto the battlefield. Then shuffle your library.")
        .SimpleAbilities(Static.Convoke)
        .Cast(p =>
        {
          p.Effect = () => new SearchLibraryPutToZone(
              zone: Zone.Battlefield,
              minCount: 0,
              maxCount: 1,
              validator: (e, c) => c.Is().Creature && c.ConvertedCost <= e.X,
              text: "Search you library for a creature card.");

          p.TimingRule(new OnEndOfOpponentsTurn());
          p.CostRule(new XIsMaxCostInYourLibrary(c => c.Is().Creature));
        });
    }
  }
}
