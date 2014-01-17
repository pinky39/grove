namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.CostRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Zones;

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