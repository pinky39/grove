namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class BloodstainedMire : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Bloodstained Mire")
        .Type("Land")
        .Text("{T}, Pay 1 life, Sacrifice Bloodstained Mire: Search your library for a Swamp or Mountain card and put it onto the battlefield. Then shuffle your library.")
        .FlavorText("Where dragons once triumphed, their bones now molder.")
        .ActivatedAbility(p =>
        {
          p.Text = "{T}, Pay 1 life, Sacrifice Bloodstained Mire: Search your library for a Swamp or Mountain card and put it onto the battlefield. Then shuffle your library.";

          p.Cost = new AggregateCost(
            new Tap(),
            new PayLife(1),
            new Sacrifice());

          p.Effect = () => new SearchLibraryPutToZone(
              zone: Zone.Battlefield,
              minCount: 0,
              maxCount: 1,
              validator: (c, ctx) => c.Is("swamp") || c.Is("mountain"),
              text: "Search your library for a Swamp or Mountain card.");

          p.TimingRule(new OnFirstMain());
        });
    }
  }
}
