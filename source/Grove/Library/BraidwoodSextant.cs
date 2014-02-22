namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

  public class BraidwoodSextant : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Braidwood Sextant")
        .ManaCost("{1}")
        .Type("Artifact")
        .Text(
          "{2},{T}, Sacrifice Braidwood Sextant: Search your library for a basic land card, reveal that card, and put it into your hand. Then shuffle your library.")
        .OverrideScore(new ScoreOverride
          {Hand = ScoreCalculator.LandInHandCost - 20, Battlefield = ScoreCalculator.LandInHandCost - 10})
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text =
              "{2},{T}, Sacrifice Braidwood Sextant: Search your library for a basic land card, reveal that card, and put it into your hand. Then shuffle your library.";

            p.Cost = new AggregateCost(
              new PayMana(2.Colorless(), ManaUsage.Abilities),
              new Tap(),
              new Sacrifice());

            p.Effect = () => new SearchLibraryPutToZone(
              zone: Zone.Hand,
              minCount: 0,
              maxCount: 1,
              validator: (e, c) => c.Is().BasicLand,
              text: "Search your library for basic land card.");

            p.TimingRule(new OnFirstMain());
          });
    }
  }
}