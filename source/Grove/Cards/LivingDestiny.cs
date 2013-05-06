namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Targeting;

  public class LivingDestiny : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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

            p.Effect = () => new ControllerGainsLife(P(e => e.Target.Card().ManaCost.Converted));
            p.TargetSelector.AddCost(trg => trg.Is.Creature().In.OwnersHand());
            p.TimingRule(new EndOfTurn());
            p.TargetingRule(new OrderByRank(c => -c.ConvertedCost));
          });
    }
  }
}