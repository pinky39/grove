namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Targeting;

  public class LivingDestiny : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Living Destiny")
        .ManaCost("{3}{G}")
        .Type("Instant")
        .Text(
          "As an additional cost to cast Living Destiny, reveal a creature card from your hand.{EOL}You gain life equal to the revealed card's converted mana cost.")
        .FlavorText("'That our enemies are great only brings us greater hope.'")
        .Timing(Timings.EndOfTurn())
        .AdditionalCost<Reveal>()
        .Effect<GainLife>(e => e.Amount = e.CostTarget().Card().ManaCost.Converted)
        .Targets(
          TargetSelectorAi.GreatestConvertedManaCost(),
          costValidator: TargetValidator(
            TargetIs.Card(card => card.Is().Creature), 
            ZoneIs.OwnersHand(), 
            mustBeTargetable: false)
        );
    }
  }
}