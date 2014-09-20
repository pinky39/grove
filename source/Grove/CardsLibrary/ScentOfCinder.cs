namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using System.Linq;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Effects;

  public class ScentOfCinder : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Scent of Cinder")
        .ManaCost("{1}{R}")
        .Type("Sorcery")
        .Text(
          "Reveal any number of red cards in your hand. Scent of Cinder deals X damage to target creature or player, where X is the number of cards revealed this way.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargetForEachRevealedCard(c => c.HasColor(CardColor.Red));
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

            p.TimingRule(new WhenYourHandCountIs(minCount: 1, selector: c => c.HasColor(CardColor.Red)));
            p.TargetingRule(new EffectDealDamage(tp => tp.Controller.Hand.Count(c => c.HasColor(CardColor.Red))));
          });
    }
  }
}