namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;

  public class GaeasBounty : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
            p.Effect = () => new SearchLibraryPutToHand(
              minCount: 0,
              maxCount: 2,
              validator: c => c.Is("forest"),
              text: "Search you library for up to 2 forest cards.");

            p.TimingRule(new FirstMain());
          });
    }
  }
}