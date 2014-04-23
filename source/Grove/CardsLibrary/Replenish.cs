namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class Replenish : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Replenish")
        .ManaCost("{3}{W}")
        .Type("Sorcery")
        .Text(
          "Return all enchantment cards from your graveyard to the battlefield. (Auras with nothing to enchant remain in your graveyard.)")
        .FlavorText("Treasures, trinkets, trash—the relics of the past are brought forth again.")
        .Cast(p =>
          {
            p.Effect = () => new PutCardsFromGraveyardToBattlefield(
              c => c.Is().Enchantment, eachPlayer: false);

            p.TimingRule(new WhenYourGraveyardCountIs(c => c.Is().Enchantment, minCount: 2));
          });
    }
  }
}