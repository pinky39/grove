namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;

    public class ConeOfFlame : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Cone of Flame")
                .ManaCost("{3}{R}{R}")
                .Type("Sorcery")
                .Text("Cone of Flame deals 1 damage to target creature or player, 2 damage to another target creature or player, and 3 damage to a third target creature or player.")
                .Cast(p =>
                {
                    p.Effect = () => new DealIncrementalDamagesToTargets(1, damageIncrement: 1); 

                    p.TargetSelector.AddEffect(trg =>
                    {
                        trg.Is.CreatureOrPlayer().On.Battlefield();
                        trg.Message = "Select a target creature or player that is dealt 1 damage.";
                    });
                    p.TargetSelector.AddEffect(trg =>
                    {
                        trg.Is.CreatureOrPlayer().On.Battlefield();
                        trg.Message = "Select a second target creature or player that is dealt 2 damage.";
                    });
                    p.TargetSelector.AddEffect(trg =>
                    {
                        trg.Is.CreatureOrPlayer().On.Battlefield();
                        trg.Message = "Select a third target creature or player that is dealt 3 damage.";
                    });

                    p.TargetingRule(new EffectDealDamage());
                    p.TimingRule(new WhenOpponentControllsPermanents(c => c.Is().Creature && c.Toughness < 4, minCount: 2));
                });
        }
    }
}
