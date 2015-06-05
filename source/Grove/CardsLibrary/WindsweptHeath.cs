namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class WindsweptHeath : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Windswept Heath")
        .Type("Land")
        .Text("{T}, Pay 1 life, Sacrifice Windswept Heath: Search your library for a Forest or Plains card and put it onto the battlefield. Then shuffle your library.")
        .FlavorText("Where dragons once roared, their bones now keen.")
        .ActivatedAbility(p =>
        {
          p.Text = "{T}, Pay 1 life, Sacrifice Windswept Heath: Search your library for a Forest or Plains card and put it onto the battlefield. Then shuffle your library.";

          p.Cost = new AggregateCost(
            new Tap(),
            new PayLife(1),
            new Sacrifice());

          p.Effect = () => new SearchLibraryPutToZone(
              zone: Zone.Battlefield,
              minCount: 0,
              maxCount: 1,
              validator: (c, ctx) => c.Is("forest") || c.Is("plains"),
              text: "Search your library for a Forest or Plains card.");

          p.TimingRule(new OnFirstMain());
        });
    }
  }
}
