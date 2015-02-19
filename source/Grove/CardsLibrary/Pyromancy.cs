namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using Grove.Costs;
  using Grove.Effects;
  using Grove.AI;
  using Grove.AI.TargetingRules;
  using Grove.AI.TimingRules;

  public class Pyromancy : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Pyromancy")
        .ManaCost("{2}{R}{R}")
        .Type("Enchantment")
        .Text(
          "{3}, Discard a card at random: Pyromancy deals damage to target creature or player equal to the converted mana cost of the discarded card.")
        .FlavorText("Who harnesses fire controls the world.")
        .ActivatedAbility(p =>
          {
            p.Text =
              "{3}, Discard a card at random: Pyromancy deals damage to target creature or player equal to the converted mana cost of the discarded card.";

            p.Cost = new AggregateCost(
              new PayMana(3.Colorless()),
              new DiscardRandom());

            p.Effect = () => new DealDamageToTargets(
              amount: P(e => e.Targets.Cost.First().Card().ConvertedCost,
              EvaluateAt.AfterCost));

            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.ConvertedCost > 0));
            p.TargetingRule(new EffectDealDamage(getAmount: tp => tp.Controller.Hand.Min(c => c.ConvertedCost)));
            p.TimingRule(new TargetRemovalTimingRule(EffectTag.DealDamage));
          });
    }
  }
}