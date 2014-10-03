namespace Grove.CardsLibrary
{
    using System.Collections.Generic;
    using AI.TargetingRules;
    using AI.TimingRules;
    using Effects;
    using Modifiers;

    public class BattleMastery : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card
                .Named("Battle Mastery")
                .ManaCost("{2}{W}")
                .Type("Enchantment — Aura")
                .Text("Enchant creature{EOL}Enchanted creature has double strike.{I}(It deals both first-strike and regular combat damage.){/I}")
                .FlavorText("\"Boom! Boom! Boots the size of oxcarts, then an axe like a falling sun. Elves scattered. Trees scattered. Even the hills ran for the hills!\"{EOL}—Clachan Tales")
                .Cast(p =>
                {
                    p.Effect = () => new Attach(
                      () => new AddStaticAbility(Static.DoubleStrike));

                    p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
                    p.TimingRule(new OnFirstMain());
                    p.TargetingRule(new EffectCombatEnchantment());
                });
        }
    }
}
