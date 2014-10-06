namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class Ulcerate : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Ulcerate")
                .ManaCost("{B}")
                .Type("Instant")
                .Text("Target creature gets -3/-3 until end of turn. You lose 3 life.")
                .FlavorText("\"If it were merely lethal, that would be sufficient. The art, however, is in maximizing the suffering it causes.\"—Liliana Vess")
                .Cast(p =>
                {
                    p.Effect = () => new CompoundEffect(
                        new ApplyModifiersToTargets(() => new AddPowerAndToughness(-3, -3) { UntilEot = true }) { ToughnessReduction = 3 },
                        new ChangeLife(amount: -3, forYou: true));

                    p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

                    p.TargetingRule(new EffectReduceToughness(3));
                    p.TimingRule(new TargetRemovalTimingRule(removalTag: EffectTag.ReduceToughness));
                });
        }
    }
}
