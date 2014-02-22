namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.CostRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class CitanulFlute : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Citanul Flute")
        .ManaCost("{5}")
        .Type("Artifact")
        .Text(
          "{X},{T}: Search your library for a creature card with converted mana cost X or less, reveal it, and put it into your hand. Then shuffle your library.")
        .ActivatedAbility(p =>
          {
            p.Text =
              "{X},{T}: Search your library for a creature card with converted mana cost X or less, reveal it, and put it into your hand. Then shuffle your library.";

            p.Cost = new AggregateCost(
              new PayMana(Mana.Zero, ManaUsage.Abilities, hasX: true),
              new Tap());

            p.Effect = () => new SearchLibraryPutToZone(
              zone: Zone.Hand,
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