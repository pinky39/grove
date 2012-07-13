namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Costs;
  using Core.Details.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class LivingDestiny : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Living Destiny")
        .ManaCost("{3}{G}")
        .Type("Instant")
        .Text(
          "As an additional cost to cast Living Destiny, reveal a creature card from your hand.{EOL}You gain life equal to the revealed card's converted mana cost.")
        .FlavorText("'That our enemies are great only brings us greater hope.'")
        .Timing(Timings.EndOfTurn())
        .AdditionalCost<RevealCardFromHand>()
        .Effect<GainLife>((e,_) => e.Amount = self => self.CostTarget().Card().ManaCost.Converted)
        .Targets(
          filter: TargetFilters.GreatestConvertedManaCost(),
          cost: C.Selector(Selectors.CardInHand(card => card.Is().Creature))
        );
    }
  }
}