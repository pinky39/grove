namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Zones;

  public class GaeasBounty : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Gaea's Bounty")
        .ManaCost("{2}{G}")
        .Type("Sorcery")
        .Text(
          "Search your library for up to two Forest cards, reveal those cards, and put them into your hand. Then shuffle your library.")
        .FlavorText("The forest grew back so quickly that lumbering machines were suspended in the treetops.")
        .Cast(p =>
          {
            p.Effect = () => new SearchLibraryPutToZone(
              zone: Zone.Hand,
              minCount: 0,
              maxCount: 2,
              validator: (e, c) => c.Is("forest"),
              text: "Search you library for up to 2 forest cards.");

            p.TimingRule(new OnFirstMain());
          });
    }
  }
}