namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class EnsoulArtifact : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Ensoul Artifact")
                .ManaCost("{1}{U}")
                .Type("Enchantment — Aura")
                .Text("Enchant artifact{EOL}Enchanted artifact is a creature with base power and toughness 5/5 in addition to its other types.")
                .FlavorText("Too often, some self-described genius comes up with a \"labor-saving device\" that ends up creating far more work.")
                .Cast(p =>
                {
                    p.Effect = () => new Attach(
                        () => new ChangeToCreature(
                            power: m => 5,
                            toughness: m => 5,
                            type: m => m.OwningCard.Is().Creature ? m.OwningCard.Type : m.OwningCard.Type + " Creature"));

                    p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Artifact).On.Battlefield());

                    p.TimingRule(new OnFirstMain());
                    p.TimingRule(new WhenYouHavePermanents(c => c.Is().Artifact));

                    p.TargetingRule(new EffectCombatEnchantment());
                });
        }
    }
}
