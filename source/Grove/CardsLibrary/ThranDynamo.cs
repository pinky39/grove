namespace Grove.CardsLibrary
{
  using System.Collections.Generic;

  public class ThranDynamo : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Thran Dynamo")
        .ManaCost("{4}")
        .Type("Artifact")
        .Text("{T}: Add {3} to your mana pool.")
        .FlavorText(
          "Urza's metathran children were conceived, birthed, and nurtured by an integrated system of machines.")
        .ManaAbility(p =>
          {
            p.Text = "{T}: Add {3} to your mana pool.";
            p.ManaAmount(3.Colorless());
          });
    }
  }
}