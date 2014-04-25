namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TimingRules;
  using Effects;

  public class RofellossGift : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Rofellos's Gift")
        .ManaCost("{G}")
        .Type("Sorcery")
        .Text(
          "Reveal any number of green cards in your hand. Return an enchantment card from your graveyard to your hand for each card revealed this way.")
        .FlavorText("Rise, elf. We are both of Gaea, and thus we are equal.")
        .Cast(p =>
          {
            p.Effect = () => new ReturnCardsFromGraveyardToHandForEachRevealedCard(
              revealFilter: c => c.HasColor(CardColor.Green),
              graveyardFilter: c => c.Is().Enchantment);

            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.HasColor(CardColor.Green)));
            p.TimingRule(new WhenYourGraveyardCountIs(minCount: 1, selector: c => c.Is().Enchantment));
          });
    }
  }
}