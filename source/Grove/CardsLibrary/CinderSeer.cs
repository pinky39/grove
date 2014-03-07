namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class CinderSeer : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Cinder Seer")
        .ManaCost("{3}{R}")
        .Type("Creature Human Wizard")
        .Text(
          "{2}{R},{T}: Reveal any number of red cards in your hand. Cinder Seer deals X damage to target creature or player, where X is the number of cards revealed this way.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{2}{R},{T}: Reveal any number of red cards in your hand. Cinder Seer deals X damage to target creature or player, where X is the number of cards revealed this way.";
            p.Cost = new AggregateCost(
              new PayMana("{2}{R}".Parse(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new DealDamageToTargetForEachRevealedCard(c => c.HasColor(CardColor.Red));
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectDealDamage(tp => tp.Controller.Hand.Count(c => c.HasColor(CardColor.Red))));
            p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.DealDamage));
          });
    }
  }
}