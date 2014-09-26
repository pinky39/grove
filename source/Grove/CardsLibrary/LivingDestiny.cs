namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class LivingDestiny : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
            p.Cost = new AggregateCost(
              new PayMana("{3}{G}".Parse(), ManaUsage.Spells),
              new Reveal());

            p.Effect = () => new YouGainLife(P(e => e.Target.Card().ManaCost.Converted));
            p.TargetSelector.AddCost(trg => trg.Is.Creature().In.OwnersHand());
            p.TimingRule(new OnEndOfOpponentsTurn());
            p.TargetingRule(new EffectOrCostRankBy(c => -c.ConvertedCost));
          });
    }
  }
}