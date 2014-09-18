namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using Effects;

    public class LightningStrike : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Lightning Strike")
                .ManaCost("{1}{R}")
                .Type("Instant")
                .Text("Lightning Strike deals 3 damage to target creature or player.")
                .FlavorText("To wield lightning is to tame chaos.")
                .Cast(p =>
                {
                    p.Effect = () => new DealDamageToTargets(3);
                    p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());

                    p.TargetingRule(new EffectDealDamage(3));
                });
        }
    }
}
