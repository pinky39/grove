namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Costs;
  using Core.Cards.Effects;
  using Core.Dsl;
  using Core.Mana;
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
        .Cast(p =>
          {
            p.Timing = Timings.EndOfTurn();
            p.Cost = Cost<PayMana, Reveal>(c => c.Amount = "{3}{G}".ParseMana());
            p.Effect = Effect<GainLife>(e => e.Amount = e.CostTarget().Card().ManaCost.Converted);
            p.CostTargets = L(Target(Validators.Card(card => card.Is().Creature), Zones.OwnersHand(),
              mustBeTargetable: false));
            p.TargetSelectorAi = TargetSelectorAi.GreatestConvertedManaCost();
          });
    }
  }
}