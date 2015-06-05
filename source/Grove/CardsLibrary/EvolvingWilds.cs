namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class EvolvingWilds : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Evolving Wilds")
        .Type("Land")
        .Text("{T}, Sacrifice Evolving Wilds: Search your library for a basic land card and put it onto the battlefield tapped. Then shuffle your library.")
        .FlavorText("Without the interfering hands of civilization, nature will always shape itself to its own needs.")
        .ActivatedAbility(p =>
        {
          p.Text = "{T}, Sacrifice Evolving Wilds: Search your library for a basic land card and put it onto the battlefield tapped. Then shuffle your library.";

          p.Cost = new AggregateCost(
            new Tap(),
            new Sacrifice());

          p.Effect = () => new SearchLibraryPutToZone(
              zone: Zone.Battlefield,
              afterPutToZone: (c, g) => c.Tap(),
              minCount: 0,
              maxCount: 1,
              validator: (c, ctx) => c.Is().BasicLand,
              text: "Search your library for a basic land card.");

          p.TimingRule(new OnFirstMain());
        });
    }
  }
}
